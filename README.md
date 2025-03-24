# NBitcoin.BIP85TestVectors

This project is a self-contained test suite to validate the correctness of BIP85 implementations using the [official BIP85 test vectors](https://github.com/bitcoin/bips/blob/master/bip-0085.mediawiki#test-vectors).

It uses [NBitcoin](https://github.com/MetacoSA/NBitcoin) to perform BIP32/HMAC operations and verifies:
- BIP39 Mnemonics (12, 18, 24 words)
- HD Seed WIF derivation (Application 2′)
- Extended Private Key (XPRV, Application 32′)
- Raw HEX Entropy (Application 128169′)

## How to Run

```bash
dotnet run
```

All test vectors are exact matches from the BIP85 specification. This ensures compatibility and correctness of derivation logic.

## Files

- `Program.cs` – Test harness using official test vectors
- `Bip85.cs` – BIP85 derivation logic using NBitcoin
