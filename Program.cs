// Description: Runs official BIP85 test vectors using NBitcoin and Bip85Lib
// Source: https://github.com/bitcoin/bips/blob/master/bip-0085.mediawiki#test-vectors
using System;
using Bip85Lib;
using NBitcoin;

    class Program
    {
        static void Main()
        {
            Console.WriteLine("Running BIP85 Full Spec Test Suite (Verbose Mode)\n");

            RunTestCase1(); // Mnemonic-only test case
            RunTestCase2(); // WIF, XPRV, HEX test case
        }

        // Test Case 1: Derives 12, 18, and 24-word BIP39 mnemonics from a fixed master xprv
        static void RunTestCase1()
        {
            Console.WriteLine("Test Case 1 – BIP39 Mnemonics Only");

            // Official BIP85 test vector: Master BIP32 root key
            var xprv = "xprv9s21ZrQH143K2LBWUUQRFXhucrQqBpKdRRxNVq2zBqsx8HVqFk2uYo8kmbaLLHRdqtQpUm98uKfu3vca1LqdGhUtyoFnCNkfmXRyPXLjbKb";
            var bip85 = new Bip85(xprv, Network.Main);

            // 12-word mnemonic from m/83696968'/39'/0'/12'/0'
            AssertEqual("12-word mnemonic",
                "girl mad pet galaxy egg matter matrix prison refuse sense ordinary nose",
                bip85.DeriveBip39Mnemonic(12, 0, Language.English));

            // 18-word mnemonic from m/83696968'/39'/0'/18'/0'
            AssertEqual("18-word mnemonic",
                "near account window bike charge season chef number sketch tomorrow excuse sniff circle vital hockey outdoor supply token",
                bip85.DeriveBip39Mnemonic(18, 0, Language.English));

            // 24-word mnemonic from m/83696968'/39'/0'/24'/0'
            AssertEqual("24-word mnemonic",
                "puppy ocean match cereal symbol another shed magic wrap hammer bulb intact gadget divorce twin tonight reason outdoor destroy simple truth cigar social volcano",
                bip85.DeriveBip39Mnemonic(24, 0, Language.English));

            Console.WriteLine();
        }

        // Test Case 2: Derives WIF, XPRV, and HEX entropy from a fixed master key
        static void RunTestCase2()
        {
            Console.WriteLine("Test Case 2 – WIF, XPRV, and HEX entropy");

            // Same root key used in spec (note: reused across cases)
            var xprv = "xprv9s21ZrQH143K2LBWUUQRFXhucrQqBpKdRRxNVq2zBqsx8HVqFk2uYo8kmbaLLHRdqtQpUm98uKfu3vca1LqdGhUtyoFnCNkfmXRyPXLjbKb";
            var bip85 = new Bip85(xprv, Network.Main);

            // WIF: m/83696968'/2'/0'
            AssertEqual("WIF",
                "Kzyv4uF39d4Jrw2W7UryTHwZr1zQVNk4dAFyqE6BuMrMh1Za7uhp",
                bip85.DeriveWif(0));

            // XPRV: m/83696968'/32'/0'
            // Note: BIP85 reverses BIP32's entropy layout (chain code first, then key)
            AssertEqual("XPRV",
                "xprv9s21ZrQH143K2srSbCSg4m4kLvPMzcWydgmKEnMmoZUurYuBuYG46c6P71UGXMzmriLzCCBvKQWBUv3vPB3m1SATMhp3uEjXHJ42jFg7myX",
                bip85.DeriveXprv(0));

            // HEX: m/83696968'/128169'/64'/0'
            // Uses full 64 bytes of entropy and truncates to 64 bytes as requested
            AssertEqual("HEX entropy (64-byte)",
                "492db4698cf3b73a5a24998aa3e9d7fa96275d85724a91e71aa2d645442f878555d078fd1f1f67e368976f04137b1f7a0d19232136ca50c44614af72b5582a5c",
                bip85.DeriveHexEntropy(64, 0));

            Console.WriteLine();
        }

        // Assertion helper: displays pass/fail and expected/actual if they differ
        static void AssertEqual(string label, string expected, string actual)
        {
            if (expected == actual)
            {
                Console.WriteLine($"{label} – Passed");
            }
            else
            {
                Console.WriteLine($"{label} – Failed");
                Console.WriteLine($"Expected: {expected}");
                Console.WriteLine($"Actual:   {actual}");
            }
        }
    }

