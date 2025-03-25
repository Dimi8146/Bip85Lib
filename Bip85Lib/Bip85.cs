using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using NBitcoin;
using NBitcoin.Crypto;


namespace Bip85Lib
{
    public class Bip85
    {
        private readonly ExtKey _masterKey;
        private readonly Network _network;

        private static readonly Dictionary<Language, Wordlist> CachedWordlists = new();
	    private static readonly Dictionary<Language, int> LanguageIndexes = new()
    	{
    	    { Language.English, 0 },
            { Language.Japanese, 1 },
//nosupport { Language.Korean, 2 },
            { Language.Spanish, 3 },
            { Language.ChineseSimplified, 4 },
            { Language.ChineseTraditional, 5 },
            { Language.French, 6 },
//nosupport { Language.Italian, 7 },
            { Language.Czech, 8 },
	    { Language.PortugueseBrazil, 9 },
	};

        public Bip85(string xprv, Network network)
        {
            _masterKey = ExtKey.Parse(xprv, network);
            _network = network;
        }

        private static Wordlist LoadWordlist(Language language)
        {
            if (!CachedWordlists.TryGetValue(language, out var wordlist))
            {
                wordlist = Wordlist.LoadWordList(language).GetAwaiter().GetResult();
                CachedWordlists[language] = wordlist;
            }
            return wordlist;
        }

        private byte[] DeriveEntropyFromPath(KeyPath path, int bits)
        {
            var derivedKey = _masterKey.Derive(path);
            var keyBytes = derivedKey.PrivateKey.ToBytes();

            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes("bip-entropy-from-k"));
            return hmac.ComputeHash(keyBytes).Take(bits / 8).ToArray();
        }

        private byte[] DeriveEntropyFromPathRaw(KeyPath path)
        {
            var derivedKey = _masterKey.Derive(path);
            var keyBytes = derivedKey.PrivateKey.ToBytes();

            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes("bip-entropy-from-k"));
            return hmac.ComputeHash(keyBytes);
        }

        public string DeriveBip39Mnemonic(int wordCount, int index, Language language)
        {
            int entropyBits = wordCount switch
            {
                12 => 128,
                15 => 160,
                18 => 192,
                21 => 224,
                24 => 256,
                _ => throw new ArgumentException("Invalid BIP39 word count")
            };

            var wordlist = LoadWordlist(language);
            var languageIndex = LanguageIndexes[language];
            var path = new KeyPath($"m/83696968'/39'/{languageIndex}'/{wordCount}'/{index}'");
            var entropy = DeriveEntropyFromPath(path, entropyBits);
            return new Mnemonic(wordlist, entropy).ToString();
        }

        public string DeriveWif(int index)
        {
            var path = new KeyPath($"m/83696968'/2'/{index}'");
            var entropy = DeriveEntropyFromPath(path, 256);
            return new Key(entropy).GetWif(_network).ToString();
        }

        public string DeriveXprv(int index)
        {
            var path = new KeyPath($"m/83696968'/32'/{index}'");
            var fullEntropy = DeriveEntropyFromPathRaw(path); // 64 bytes

            if (fullEntropy.Length != 64)
                throw new Exception("Expected 64 bytes for XPRV derivation");

            // According to spec: first 32 = chain code, next 32 = private key
            var chainCode = fullEntropy.Take(32).ToArray();
            var key = new Key(fullEntropy.Skip(32).ToArray());

            var extKey = new ExtKey(key, chainCode);
            return extKey.ToString(_network);
        }

        public string DeriveHexEntropy(int numBytes, int index)
        {
            if (numBytes < 16 || numBytes > 64)
                throw new ArgumentException("HEX derivation requires 16 <= numBytes <= 64");

            var path = new KeyPath($"m/83696968'/128169'/{numBytes}'/{index}'");
            var fullEntropy = DeriveEntropyFromPathRaw(path);
            var entropy = fullEntropy.Take(numBytes).ToArray();

            return BitConverter.ToString(entropy).Replace("-", "").ToLowerInvariant();
        }

        public static ExtKey DeriveExtKeyFromMnemonic(string mnemonicPhrase)
        {
            var mnemonic = new Mnemonic(mnemonicPhrase);
            return mnemonic.DeriveExtKey();
        }

        public static string GetFingerprint(ExtKey extKey)
        {
            var pubKeyBytes = extKey.Neuter().PubKey.ToBytes();
            var sha256 = SHA256.Create().ComputeHash(pubKeyBytes);
            var ripemd160Hash = Hashes.RIPEMD160(sha256, sha256.Length);
            return BitConverter.ToString(ripemd160Hash.Take(4).ToArray()).Replace("-", "").ToLowerInvariant();
        }

        public static string GetXpub(ExtKey extKey, Network network)
        {
            return extKey.Neuter().ToString(network);
        }
    }
}
