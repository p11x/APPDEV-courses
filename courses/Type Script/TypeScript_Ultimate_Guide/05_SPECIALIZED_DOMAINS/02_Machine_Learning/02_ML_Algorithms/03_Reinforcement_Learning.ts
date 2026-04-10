/**
 * Category: 05_SPECIALIZED_DOMAINS
 * Subcategory: 02_Machine_Learning
 * Concept: 02_ML_Algorithms
 * Topic: 03_Reinforcement_Learning
 * Purpose: Define reinforcement learning algorithm types
 * Difficulty: expert
 * UseCase: machine learning
 * Version: TypeScript 5.0+
 * Compatibility: Node.js 18+, Modern Browsers
 * Performance: O(s*a) Q-learning, O(n) policy gradient
 * Security: Reward clipping prevents exploitation
 */

namespace ReinforcementLearningTypes {
  export type RLAlgorithm = 'value_based' | 'policy_gradient' | 'actor_critic' | 'model_based';

  export interface RLEnvironment {
    stateSpace: Space;
    actionSpace: Space;
    reset(): State;
    step(action: Action): StepResult;
    render(): void;
    close(): void;
  }

  export interface Space {
    type: 'discrete' | 'continuous';
    shape: number[];
    low?: number[];
    high?: number[];
    n?: number;
  }

  export interface State {
    observation: number[];
    info: Record<string, unknown>;
  }

  export interface Action {
    discrete?: number;
    continuous?: number[];
  }

  export interface StepResult {
    state: State;
    reward: number;
    done: boolean;
    truncated: boolean;
  }

  export interface Policy {
    selectAction(state: State): Action;
    update(transitions: Transition[]): void;
    save(path: string): Promise<void>;
    load(path: string): Promise<void>;
  }

  export interface Transition {
    state: State;
    action: Action;
    reward: number;
    nextState: State;
    done: boolean;
  }

  export interface QLearning extends Policy {
    algorithm: 'q_learning' | 'double_q_learning' | 'dueling_q';
    qTable: Map<string, number[]>;
    learningRate: number;
    discountFactor: number;
    epsilon: number;
    epsilonDecay: number;
    epsilonMin: number;
    actionSpace: Space;
  }

  export interface DQN extends Policy {
    algorithm: 'dqn' | 'double_dqn' | 'prioritized_dqn';
    qNetwork: NeuralNetwork;
    targetNetwork: NeuralNetwork;
    memory: ReplayBuffer;
    learningRate: number;
    discountFactor: number;
    epsilon: number;
    targetUpdateFreq: number;
    batchSize: number;
  }

  export interface ReplayBuffer {
    capacity: number;
    buffer: Transition[];
    push(transition: Transition): void;
    sample(batchSize: number): Transition[];
    isFull(): boolean;
  }

  export interface PolicyGradient extends Policy {
    algorithm: 'reinforce' | 'actor_critic' | 'a2c' | 'ppo';
    policyNetwork: NeuralNetwork;
    baselineNetwork?: NeuralNetwork;
    learningRate: number;
    discountFactor: number;
    entropyCoef: number;
    valueCoef: number;
    maxGradNorm: number;
  }

  export interface PPO extends Policy {
    algorithm: 'ppo';
    actor: NeuralNetwork;
    critic: NeuralNetwork;
    clipRatio: number;
    epochs: number;
    batchSize: number;
    gamma: number;
    lam: number;
    valueLossCoef: number;
    entropyCoef: number;
    maxGradNorm: number;
  }

  export interface ActorCritic extends Policy {
    algorithm: 'a2c' | 'a3c';
    actor: NeuralNetwork;
    critic: NeuralNetwork;
    sharedLayers?: NeuralNetwork;
    updateType: 'sync' | 'async';
    learningRate: number;
    gamma: number;
    valueLossCoef: number;
    entropyCoef: number;
  }

  export interface MonteCarloTreeSearch extends Policy {
    algorithm: 'mcts';
    root: TreeNode;
    iterations: number;
    explorationConstant: number;
    selectionPolicy: 'uct' | 'pucb' | 'grave';
    expand(state: State): TreeNode;
    backpropagate(node: TreeNode, reward: number): void;
  }

  export interface TreeNode {
    state: State;
    parent?: TreeNode;
    children: TreeNode[];
    visits: number;
    value: number;
    action?: Action;
  }

  export interface RLAgent {
    policy: Policy;
    env: RLEnvironment;
    train(episodes: number): TrainingHistory;
    evaluate(episodes: number): EvaluationResult;
  }

  export interface TrainingHistory {
    episodeRewards: number[];
    episodeLengths: number[];
    losses: number[];
  }

  export interface EvaluationResult {
    meanReward: number;
    stdReward: number;
    meanEpisodeLength: number;
    successRate: number;
  }
}

// Cross-reference: 01_Supervised_Learning.ts, 02_Unsupervised_Learning.ts
console.log("\n=== Reinforcement Learning Types ===");
console.log("Related: 01_Supervised_Learning.ts, 02_Unsupervised_Learning.ts");