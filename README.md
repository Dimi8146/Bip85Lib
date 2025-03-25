# Bip85Lib

This project provides a focused test suite to validate a new C# BIP85 implementation against the [official BIP85 specification](https://github.com/bitcoin/bips/blob/master/bip-0085.mediawiki#test-vectors) utilizing the NBitcoin libraries.
- ⚠️ Multilanguage support attempted.
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
git clone https://github.com/Dimi8146/Bip85Lib
cd Bip85Lib
dotnet restore
dotnet run
```
Successful result:
```
Running BIP85 Full Spec Test Suite (Verbose Mode)

Test Case 1 – BIP39 Mnemonics Only
12-word mnemonic – Passed
18-word mnemonic – Passed
24-word mnemonic – Passed

Test Case 2 – WIF, XPRV, and HEX entropy
WIF – Passed
XPRV – Passed
HEX entropy (64-byte) – Passed
```
