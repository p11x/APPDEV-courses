/**
 * @group unit
 * @group advanced-topics
 */
import { expect, fixture, html } from '@open-wc/testing';
import './13_7_Blockchain-Integration.js';

describe('Blockchain Integration', () => {
  it('should have ETH_NETWORKS', () => {
    expect(ETH_NETWORKS).to.exist;
    expect(ETH_NETWORKS.mainnet.chainId).to.equal('0x1');
    expect(ETH_NETWORKS.goerli.name).to.equal('Goerli Testnet');
    expect(ETH_NETWORKS.polygon.name).to.equal('Polygon Mainnet');
  });

  it('should have DEFAULT_CONFIG', () => {
    expect(DEFAULT_CONFIG.network).to.equal('mainnet');
    expect(DEFAULT_CONFIG.chainId).to.equal('0x1');
    expect(DEFAULT_CONFIG.autoConnect).to.be.false;
  });

  it('should have sepolia network', () => {
    expect(ETH_NETWORKS.sepolia.chainId).to.equal('0xaa36a7');
  });

  it('should have arbitrum network', () => {
    expect(ETH_NETWORKS.arbitrum.name).to.equal('Arbitrum One');
  });
});

describe('Web3Provider', () => {
  let provider;

  beforeEach(() => {
    provider = new Web3Provider();
  });

  it('should create provider', () => {
    expect(provider).to.exist;
    expect(provider.isConnected).to.be.false;
  });

  it('should have config', () => {
    expect(provider.config.network).to.equal('mainnet');
  });

  it('should connect', async () => {
    await provider.connect();
    expect(provider).to.exist;
  });

  it('should have account', () => {
    expect(provider.account).to.be.null;
  });
});

describe('SmartContract', () => {
  let contract;

  beforeEach(() => {
    contract = new SmartContract('0x123', ['function transfer(address,uint256)']);
  });

  it('should create contract', () => {
    expect(contract).to.exist;
    expect(contract.address).to.equal('0x123');
  });

  it('should have abi', () => {
    expect(contract.abi).to.be.an('array');
  });

  it('should call method', async () => {
    contract.signer = { call: async () => '0x' };
    const result = await contract.call('transfer', ['0x456', 100]);
    expect(result).to.exist;
  });
});