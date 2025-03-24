# NBitcoin.BIP85TestVectors

This project provides a focused test suite to validate a new C# BIP85 implementation against the [official BIP85 specification](https://github.com/bitcoin/bips/blob/master/bip-0085.mediawiki#test-vectors) utilizing the NBitcoin libraries.
---

## ✅ What It Covers

- ✅ BIP39 Mnemonics: 12, 18, and 24 words (App `39′`)
- ✅ WIF derivation (App `2′`)
- ✅ Extended Private Key (XPRV, App `32′`, BIP85 entropy layout: chain code + key)
- ✅ Raw HEX entropy (App `128169′`, full 64 bytes, truncated to requested length)

---

## ▶️ Running the Tests

Clone and run:

```bash
git clone https://github.com/your-org/NBitcoin.BIP85TestVectors.git
cd NBitcoin.BIP85TestVectors
dotnet restore
dotnet run
```
