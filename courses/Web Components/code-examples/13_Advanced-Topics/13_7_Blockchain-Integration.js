/**
 * Blockchain Integration - Web3 and Blockchain Components
 * @description Connect to Ethereum, Polygon, Solana blockchains and interact with smart contracts
 * @module advanced/blockchain-integration
 * @version 1.0.0
 */

(function() {
  'use strict';

  const ETH_NETWORKS = {
    mainnet: { chainId: '0x1', name: 'Ethereum Mainnet', rpcUrl: 'https://eth-mainnet.g.alchemy.com/v2/demo' },
    goerli: { chainId: '0x5', name: 'Goerli Testnet', rpcUrl: 'https://eth-goerli.g.alchemy.com/v2/demo' },
    sepolia: { chainId: '0xaa36a7', name: 'Sepolia Testnet', rpcUrl: 'https://eth-sepolia.g.alchemy.com/v2/demo' },
    polygon: { chainId: '0x89', name: 'Polygon Mainnet', rpcUrl: 'https://polygon-rpc.com' },
    mumbai: { chainId: '0x13881', name: 'Mumbai Testnet', rpcUrl: 'https://rpc-mumbai.maticvigil.com' },
    arbitrum: { chainId: '0xa4b1', name: 'Arbitrum One', rpcUrl: 'https://arb1.arbitrum.io/rpc' },
    optimism: { chainId: '0xa', name: 'Optimism', rpcUrl: 'https://mainnet.optimism.io' }
  };

  const DEFAULT_CONFIG = {
    network: 'mainnet',
    chainId: '0x1',
    autoConnect: false,
    requestAccounts: true
  };

  class Web3Provider {
    constructor(config = {}) {
      this.config = { ...DEFAULT_CONFIG, ...config };
      this.provider = null;
      this.signer = null;
      this.chainId = null;
      this.account = null;
      this.isConnected = false;
    }

    async connect() {
      if (typeof window.ethereum !== 'undefined') {
        this.provider = window.ethereum;
        return this.connectToInjected();
      }

      const network = ETH_NETWORKS[this.config.network];
      if (network) {
        this.provider = new ethers.providers.JsonRpcProvider(network.rpcUrl);
        this.chainId = network.chainId;
        this.isConnected = true;
        return this.provider;
      }

      throw new Error('No Web3 provider available');
    }

    async connectToInjected() {
      try {
        if (this.config.requestAccounts) {
          const accounts = await this.provider.request({ method: 'eth_requestAccounts' });
          this.account = accounts[0];
        }

        const chainId = await this.provider.request({ method: 'eth_chainId' });
        this.chainId = chainId;

        this.provider.on('accountsChanged', (accounts) => {
          this.account = accounts[0];
          this.dispatchEvent(new CustomEvent('account-change', {
            detail: { account: this.account }
          }));
        });

        this.provider.on('chainChanged', (chainId) => {
          this.chainId = chainId;
          this.dispatchEvent(new CustomEvent('chain-change', {
            detail: { chainId }
          }));
        });

        this.isConnected = true;
        return this.provider;
      } catch (error) {
        console.error('Failed to connect to injected provider:', error);
        throw error;
      }
    }

    async switchNetwork(chainId) {
      const chainConfig = Object.values(ETH_NETWORKS).find(n => n.chainId === chainId);
      if (!chainConfig) return false;

      try {
        await this.provider.request({
          method: 'wallet_switchEthereumChain',
          params: [{ chainId }]
        });
        return true;
      } catch (error) {
        if (error.code === 4902) {
          await this.addNetwork(chainConfig);
          return true;
        }
        throw error;
      }
    }

    async addNetwork(network) {
      await this.provider.request({
        method: 'wallet_addEthereumChain',
        params: [{
          chainId: network.chainId,
          chainName: network.name,
          nativeCurrency: { name: 'ETH', symbol: 'ETH', decimals: 18 },
          rpcUrls: [network.rpcUrl]
        }]
      });
    }

    async getBalance(address) {
      if (!this.provider) await this.connect();
      const balance = await this.provider.getBalance(address);
      return ethers.utils.formatEther(balance);
    }

    async getNetwork() {
      if (!this.provider) await this.connect();
      const network = await this.provider.getNetwork();
      return network;
    }

    getAccount() {
      return this.account;
    }

    isMetamaskInstalled() {
      return typeof window.ethereum !== 'undefined' && window.ethereum.isMetaMask;
    }
  }

  class SmartContract {
    constructor(provider, abi, address) {
      this.provider = provider;
      this.abi = abi;
      this.address = address;
      this.contract = null;
      this.initialize();
    }

    initialize() {
      if (typeof ethers !== 'undefined') {
        this.contract = new ethers.Contract(this.address, this.abi, this.provider);
      }
    }

    async call(method, ...args) {
      if (!this.contract) throw new Error('Contract not initialized');
      return this.contract[method](...args);
    }

    async estimateGas(method, ...args) {
      if (!this.contract) throw new Error('Contract not initialized');
      return this.contract.estimateGas[method](...args);
    }

    async send(method, ...args) {
      if (!this.contract) throw new Error('Contract not initialized');
      const signer = this.provider.getSigner();
      const contractWithSigner = this.contract.connect(signer);
      return contractWithSigner[method](...args);
    }

    on(event, callback) {
      if (!this.contract) return;
      this.contract.on(event, callback);
    }

    off(event, callback) {
      if (!this.contract) return;
      this.contract.off(event, callback);
    }
  }

  class ERC20Token {
    constructor(provider, tokenAddress) {
      this.provider = provider;
      this.tokenAddress = tokenAddress;
      this.contract = null;
      
      const ABI = [
        'function name() view returns (string)',
        'function symbol() view returns (string)',
        'function decimals() view returns (uint8)',
        'function totalSupply() view returns (uint256)',
        'function balanceOf(address owner) view returns (uint256)',
        'function transfer(address to, uint256 amount)',
        'function allowance(address owner, address spender) view returns (uint256)',
        'function approve(address spender, uint256 amount)',
        'function transferFrom(address from, address to, uint256 amount)',
        'event Transfer(address indexed from, address indexed to, uint256 value)',
        'event Approval(address indexed owner, address indexed spender, uint256 value)'
      ];
      
      this.contract = new SmartContract(provider, ABI, tokenAddress);
    }

    async getName() {
      return this.contract.call('name');
    }

    async getSymbol() {
      return this.contract.call('symbol');
    }

    async getDecimals() {
      return this.contract.call('decimals');
    }

    async getBalanceOf(address) {
      const balance = await this.contract.call('balanceOf', address);
      return this.formatUnits(balance);
    }

    async getTotalSupply() {
      const supply = await this.contract.call('totalSupply');
      return this.formatUnits(supply);
    }

    async transfer(to, amount) {
      const value = this.parseUnits(amount);
      return this.contract.send('transfer', to, value);
    }

    async approve(spender, amount) {
      const value = this.parseUnits(amount);
      return this.contract.send('approve', spender, value);
    }

    async allowance(owner, spender) {
      const allowance = await this.contract.call('allowance', owner, spender);
      return this.formatUnits(allowance);
    }

    parseUnits(amount, decimals = 18) {
      return ethers.utils.parseUnits(String(amount), decimals);
    }

    formatUnits(amount, decimals = 18) {
      return ethers.utils.formatUnits(amount, decimals);
    }
  }

  class NFTToken {
    constructor(provider, tokenAddress) {
      this.provider = provider;
      this.tokenAddress = tokenAddress;
      
      const ABI = [
        'function name() view returns (string)',
        'function symbol() view returns (string)',
        'function tokenURI(uint256 tokenId) view returns (string)',
        'function ownerOf(uint256 tokenId) view returns (address)',
        'function balanceOf(address owner) view returns (uint256)',
        'function tokenOfOwnerByIndex(address owner, uint256 index) view returns (uint256)',
        'function totalSupply() view returns (uint256)',
        'function metadata(uint256 tokenId) view returns (string)',
        'event Transfer(address indexed from, address indexed to, uint256 indexed tokenId)'
      ];
      
      this.contract = new SmartContract(provider, ABI, tokenAddress);
    }

    async getName() {
      return this.contract.call('name');
    }

    async getSymbol() {
      return this.contract.call('symbol');
    }

    async getTokenURI(tokenId) {
      return this.contract.call('tokenURI', tokenId);
    }

    async getOwnerOf(tokenId) {
      return this.contract.call('ownerOf', tokenId);
    }

    async getBalanceOf(address) {
      return this.contract.call('balanceOf', address);
    }

    async getTokensOfOwner(address) {
      const balance = await this.contract.call('balanceOf', address);
      const tokens = [];
      
      for (let i = 0; i < balance.toNumber(); i++) {
        const tokenId = await this.contract.call('tokenOfOwnerByIndex', address, i);
        tokens.push(tokenId.toNumber());
      }
      
      return tokens;
    }
  }

  class WalletConnector extends HTMLElement {
    static get observedAttributes() {
      return ['auto-connect', 'chain'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._provider = null;
      this._account = null;
      this._chainId = null;
      this._connected = false;
    }

    static get observedAttributes() {
      return ['auto-connect', 'chain', 'show-balance'];
    }

    connectedCallback() {
      this._provider = new Web3Provider();
      
      if (this.hasAttribute('auto-connect')) {
        this.connect();
      }
      
      this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue) {
        if (name === 'chain') {
          this.switchChain(newValue);
        }
      }
    }

    async connect() {
      try {
        await this._provider.connect();
        this._account = this._provider.getAccount();
        this._chainId = this._provider.chainId;
        this._connected = true;
        
        this.dispatchEvent(new CustomEvent('wallet-connect', {
          detail: { account: this._account, chainId: this._chainId },
          bubbles: true,
          composed: true
        }));
        
        if (this.hasAttribute('show-balance') && this._account) {
          this.updateBalance();
        }
        
        this.updateUI();
      } catch (error) {
        this.dispatchEvent(new CustomEvent('wallet-error', {
          detail: { error },
          bubbles: true,
          composed: true
        }));
      }
    }

    async disconnect() {
      this._account = null;
      this._connected = false;
      this.updateUI();
    }

    async switchChain(chainId) {
      if (this._provider) {
        await this._provider.switchNetwork(chainId);
      }
    }

    async updateBalance() {
      if (!this._account) return;
      const balance = await this._provider.getBalance(this._account);
      this._balance = balance;
      this.updateUI();
    }

    get account() {
      return this._account;
    }

    get shortAddress() {
      if (!this._account) return '';
      return `${this._account.slice(0, 6)}...${this._account.slice(-4)}`;
    }

    updateUI() {
      const status = this.shadowRoot.querySelector('.wallet-status');
      const address = this.shadowRoot.querySelector('.wallet-address');
      const balance = this.shadowRoot.querySelector('.wallet-balance');
      const connectBtn = this.shadowRoot.querySelector('.connect-button');
      
      if (status) {
        status.textContent = this._connected ? 'Connected' : 'Not Connected';
        status.classList.toggle('connected', this._connected);
      }
      
      if (address && this._account) {
        address.textContent = this.shortAddress;
      }
      
      if (balance && this._balance) {
        balance.textContent = `${parseFloat(this._balance).toFixed(4)} ETH`;
      }
      
      if (connectBtn) {
        connectBtn.textContent = this._connected ? 'Disconnect' : 'Connect Wallet';
      }
    }

    render() {
      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
          }
          .wallet-container {
            padding: 16px;
            font-family: system-ui, -apple-system, sans-serif;
            border: 1px solid var(--color-border, #e5e7eb);
            border-radius: 8px;
            background: var(--color-background, #fff);
          }
          .wallet-status {
            display: inline-block;
            width: 8px;
            height: 8px;
            border-radius: 50%;
            background: #ef4444;
            margin-right: 8px;
          }
          .wallet-status.connected {
            background: #10b981;
          }
          .wallet-info {
            margin-top: 8px;
          }
          .wallet-address {
            font-family: monospace;
            font-size: 14px;
          }
          .wallet-balance {
            color: var(--color-text-secondary, #6b7280);
            font-size: 14px;
          }
          .connect-button {
            padding: 8px 16px;
            background: #667eea;
            color: white;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-size: 14px;
            margin-top: 8px;
          }
          .connect-button:hover {
            background: #5a6fd6;
          }
        </style>
        <div class="wallet-container">
          <div class="wallet-status"></div>
          <span class="wallet-status-text">Not Connected</span>
          <div class="wallet-info" hidden>
            <div class="wallet-address"></div>
            <div class="wallet-balance"></div>
          </div>
          <button class="connect-button">Connect Wallet</button>
        </div>
      `;

      const button = this.shadowRoot.querySelector('.connect-button');
      button?.addEventListener('click', () => {
        if (this._connected) {
          this.disconnect();
        } else {
          this.connect();
        }
      });
    }
  }

  class TokenBalance extends HTMLElement {
    static get observedAttributes() {
      return ['token-address', 'address'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._token = null;
      this._balance = '0';
      this._loading = false;
    }

    static get observedAttributes() {
      return ['token-address', 'address', 'show-symbol'];
    }

    connectedCallback() {
      this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue) {
        if (name === 'token-address' || name === 'address') {
          this.fetchBalance();
        }
      }
    }

    async fetchBalance() {
      const tokenAddress = this.getAttribute('token-address');
      const address = this.getAttribute('address');
      
      if (!tokenAddress || !address) return;

      this._loading = true;
      this.updateUI();

      try {
        const provider = new Web3Provider();
        await provider.connect();
        
        if (tokenAddress === '0x0000000000000000000000000000000000000000') {
          this._balance = await provider.getBalance(address);
        } else {
          const token = new ERC20Token(provider.provider, tokenAddress);
          this._balance = await token.getBalanceOf(address);
        }
      } catch (error) {
        console.error('Failed to fetch balance:', error);
        this._balance = '0';
      }

      this._loading = false;
      this.updateUI();
    }

    updateUI() {
      const balance = this.shadowRoot.querySelector('.balance');
      if (balance) {
        balance.textContent = this._loading ? 'Loading...' : this._balance;
      }
    }

    render() {
      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
          }
          .token-balance {
            padding: 8px 12px;
            font-family: system-ui, -apple-system, sans-serif;
            font-size: 14px;
          }
        </style>
        <div class="token-balance">
          <slot></slot>
          <span class="balance"></span>
        </div>
      `;
    }
  }

  class NFTCard extends HTMLElement {
    static get observedAttributes() {
      return ['token-id', 'contract'];
    }

    constructor() {
      super();
      this.attachShadow({ mode: 'open' });
      this._metadata = null;
      this._imageUrl = null;
      this._name = '';
    }

    static get observedAttributes() {
      return ['token-id', 'contract', 'show-name', 'show-description'];
    }

    connectedCallback() {
      this.render();
    }

    attributeChangedCallback(name, oldValue, newValue) {
      if (oldValue !== newValue && name === 'token-id') {
        this.fetchMetadata();
      }
    }

    async fetchMetadata() {
      const contractAddress = this.getAttribute('contract');
      const tokenId = this.getAttribute('token-id');
      
      if (!contractAddress || !tokenId) return;

      try {
        const provider = new Web3Provider();
        await provider.connect();
        
        const nft = new NFTToken(provider.provider, contractAddress);
        const uri = await nft.getTokenURI(parseInt(tokenId));
        
        const response = await fetch(uri);
        this._metadata = await response.json();
        this._name = this._metadata.name || `NFT #${tokenId}`;
        this._imageUrl = this._metadata.image;
        
        this.render();
      } catch (error) {
        console.error('Failed to fetch NFT metadata:', error);
      }
    }

    render() {
      const showName = this.hasAttribute('show-name');
      const showDescription = this.hasAttribute('show-description');

      this.shadowRoot.innerHTML = `
        <style>
          :host {
            display: block;
          }
          .nft-card {
            border: 1px solid var(--color-border, #e5e7eb);
            border-radius: 12px;
            overflow: hidden;
            background: var(--color-background, #fff);
            font-family: system-ui, -apple-system, sans-serif;
          }
          .nft-image {
            width: 100%;
            aspect-ratio: 1;
            object-fit: cover;
            background: #f3f4f6;
          }
          .nft-info {
            padding: 12px;
          }
          .nft-name {
            font-weight: 600;
            font-size: 16px;
          }
          .nft-description {
            color: var(--color-text-secondary, #6b7280);
            font-size: 14px;
            margin-top: 4px;
          }
        </style>
        <div class="nft-card">
          ${this._imageUrl ? `<img class="nft-image" src="${this._imageUrl}" alt="${this._name}" />` : '<div class="nft-image"></div>'}
          <div class="nft-info">
            ${showName ? `<div class="nft-name">${this._name}</div>` : ''}
            ${showDescription && this._metadata?.description ? `<div class="nft-description">${this._metadata.description}</div>` : ''}
            <slot></slot>
          </div>
        </div>
      `;
    }
  }

  customElements.define('wallet-connector', WalletConnector);
  customElements.define('token-balance', TokenBalance);
  customElements.define('nft-card', NFTCard);

  if (typeof window !== 'undefined') {
    window.BlockchainIntegration = {
      ETH_NETWORKS,
      Web3Provider,
      SmartContract,
      ERC20Token,
      NFTToken,
      WalletConnector,
      TokenBalance,
      NFTCard
    };
  }

  export {
    ETH_NETWORKS,
    Web3Provider,
    SmartContract,
    ERC20Token,
    NFTToken,
    WalletConnector,
    TokenBalance,
    NFTCard
  };
})();