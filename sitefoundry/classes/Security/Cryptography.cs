using System;
using System.IO;
using System.Text;
using System.Security;
using System.Security.Cryptography;

namespace Dury.SiteFoundry.Security.Cryptography
{
	/// <summary>
	/// One-way only encryption class.  Useful for password hashing and user authentication/verification
	/// </summary>
	public class AsymmetricEncryption
	{
		public static string ComputeHash(string plainText,string hashAlgorithm, byte[] saltBytes)
		{
			// If salt is not specified, generate it on the fly.
			if (saltBytes == null)
			{
				// Define min and max salt sizes.
				int minSaltSize = 4;
				int maxSaltSize = 8;

				// Generate a random number for the size of the salt.
				Random  random = new Random();
				int saltSize = random.Next(minSaltSize, maxSaltSize);

				// Allocate a byte array, which will hold the salt.
				saltBytes = new byte[saltSize];

				// Initialize a random number generator.
				RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

				// Fill the salt with cryptographically strong byte values.
				rng.GetNonZeroBytes(saltBytes); 
			}
        
			// Convert plain text into a byte array.
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        
			// Allocate array, which will hold plain text and salt.
			byte[] plainTextWithSaltBytes = 
				new byte[plainTextBytes.Length + saltBytes.Length];

			// Copy plain text bytes into resulting array.
			for (int i=0; i < plainTextBytes.Length; i++)
				plainTextWithSaltBytes[i] = plainTextBytes[i];
        
			// Append salt bytes to the resulting array.
			for (int i=0; i < saltBytes.Length; i++)
				plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

			// Because we support multiple hashing algorithms, we must define
			// hash object as a common (abstract) base class. We will specify the
			// actual hashing algorithm class later during object creation.
			HashAlgorithm hash;
        
			// Make sure hashing algorithm name is specified.
			if (hashAlgorithm == null)
				hashAlgorithm = "";
        
			// Initialize appropriate hashing algorithm class.
			switch (hashAlgorithm.ToUpper())
			{
				case "SHA1":
					hash = new SHA1Managed();
					break;

				case "SHA256":
					hash = new SHA256Managed();
					break;

				case "SHA384":
					hash = new SHA384Managed();
					break;

				case "SHA512":
					hash = new SHA512Managed();
					break;

				default:
					hash = new MD5CryptoServiceProvider();
					break;
			}
        
			// Compute hash value of our plain text with appended salt.
			byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);
        
			// Create array which will hold hash and original salt bytes.
			byte[] hashWithSaltBytes = new byte[hashBytes.Length + 
				saltBytes.Length];
        
			// Copy hash bytes into resulting array.
			for (int i=0; i < hashBytes.Length; i++)
				hashWithSaltBytes[i] = hashBytes[i];
            
			// Append salt bytes to the result.
			for (int i=0; i < saltBytes.Length; i++)
				hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];
            
			// Convert result into a base64-encoded string.
			//string hashString = Convert.ToBase64String(hashWithSaltBytes);
        
			// Return the result.
			//return hashString;
			string s= "";
			foreach (byte b in hashWithSaltBytes)
				s+= b.ToString("X");

			return s;
		}

		/// <summary>
		/// Compares a hash of the specified plain text value to a given hash
		/// value. Plain text is hashed with the same salt value as the original
		/// hash.
		/// </summary>
		/// <param name="plainText">
		/// Plain text to be verified against the specified hash. The function
		/// does not check whether this parameter is null.
		/// </param>
		/// <param name="hashAlgorithm">
		/// Name of the hash algorithm. Allowed values are: "MD5", "SHA1", 
		/// "SHA256", "SHA384", and "SHA512" (if any other value is specified,
		/// MD5 hashing algorithm will be used). This value is case-insensitive.
		/// </param>
		/// <param name="hashValue">
		/// Base64-encoded hash value produced by ComputeHash function. This value
		/// includes the original salt appended to it.
		/// </param>
		/// <returns>
		/// If computed hash mathes the specified hash the function the return
		/// value is true; otherwise, the function returns false.
		/// </returns>
		public static bool VerifyHash(string plainText, string hashAlgorithm, string hashString)
		{
			// Convert base64-encoded hash value into a byte array.
			byte[] hashWithSaltBytes = Convert.FromBase64String(hashString);
        
			// We must know size of hash (without salt).
			int hashSizeInBits, hashSizeInBytes;
        
			// Make sure that hashing algorithm name is specified.
			if (hashAlgorithm == null)
				hashAlgorithm = "";
        
			// Size of hash is based on the specified algorithm.
			switch (hashAlgorithm.ToUpper())
			{
				case "SHA1":
					hashSizeInBits = 160;
					break;

				case "SHA256":
					hashSizeInBits = 256;
					break;

				case "SHA384":
					hashSizeInBits = 384;
					break;

				case "SHA512":
					hashSizeInBits = 512;
					break;

				default: // Must be MD5
					hashSizeInBits = 128;
					break;
			}

			// Convert size of hash from bits to bytes.
			hashSizeInBytes = hashSizeInBits / 8;

			// Make sure that the specified hash value is long enough.
			if (hashWithSaltBytes.Length < hashSizeInBytes)
				return false;

			// Allocate array to hold original salt bytes retrieved from hash.
			byte[] saltBytes = new byte[hashWithSaltBytes.Length - 
				hashSizeInBytes];

			// Copy salt from the end of the hash to the new array.
			for (int i=0; i < saltBytes.Length; i++)
				saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

			// Compute a new hash string.
			string expectedHashString = 
				ComputeHash(plainText, hashAlgorithm, saltBytes);

			// If the computed hash matches the specified hash,
			// the plain text value must be correct.
			return (hashString == expectedHashString);
		}
	}


	/// <summary>
	/// 2-way encryption.  Can encrypt string into cyphertext and back when key is supplied.
	/// </summary>
	public class SymmetricEncryption 
	{ 
		private SymmetricAlgorithm moAlgorithm; 
		private int mKeySize; 

		public int KeySize 
		{ 
			get { return mKeySize; } 
			set { mKeySize = (value/8); } 
		} 

		public SymmetricAlgorithm Algorithm 
		{ 
			get { return moAlgorithm; } 
			set { moAlgorithm = value; } 
		} 

		public SymmetricEncryption() 
		{ 
			this.Algorithm = SymmetricAlgorithm.Create(); 
			int i; 
			for(i=0; i< this.Algorithm.LegalKeySizes.Length; i++) 
			{ 
				this.KeySize = this.Algorithm.LegalKeySizes[i].MinSize; 
				if(this.Algorithm.ValidKeySize(this.KeySize)) 
					break; 
			} 
			this.Algorithm.Mode = CipherMode.ECB; 
		} 

		public SymmetricEncryption(SymmetricAlgorithm Algorithm) 
		{ 
			this.Algorithm = Algorithm; 
			int i; 
			for(i=0; i< this.Algorithm.LegalKeySizes.Length; i++) 
			{ 
				this.KeySize = this.Algorithm.LegalKeySizes[i].MinSize; 
				if(this.Algorithm.ValidKeySize(this.KeySize)) 
					break; 
			} 

		} 

		public SymmetricEncryption(SymmetricAlgorithm Algorithm, int KeySize) 
		{ 
			this.Algorithm = Algorithm; 
			int i; 
			if(!this.Algorithm.ValidKeySize(KeySize)) 
			{ 
				for(i=0; i< this.Algorithm.LegalKeySizes.Length; i++) 
				{ 
					this.KeySize = this.Algorithm.LegalKeySizes[i].MinSize; 
					if(this.Algorithm.ValidKeySize(this.KeySize)) 
						break; 
				} 
			} 
		} 

		public string Encode(string Input) 
		{ 
			//return Convert.ToBase64String(StringToByteArray(Input)); 
			return Dury.SiteFoundry.Utils.HexEncoding.ToString(StringToByteArray(Input));
		} 

		public string Decode(string Input) 
		{ 
            return ByteArrayToString(Dury.SiteFoundry.Utils.HexEncoding.GetBytes(Input));
			//return ByteArrayToString(Convert.FromBase64String(Input)); 
		} 

		public string DecryptString(string Input, string Key) 
		{ 
			return DecryptString(Input, Key, Key); 
		} 


		public string EncryptString(string Input, string Key) 
		{ 
			return EncryptString(Input, Key, Key); 
		} 

		public string EncryptString(string Input, string Key, string IV) 
		{ 
			try 
			{ 
				if(Key.Length == 0) 
					return ""; 

				while(Key.Length < this.KeySize) 
					Key += Key; 
				Key = Key.Substring(0, this.KeySize); 

				if(IV.Length == 0) 
					IV = Key; 
				while(IV.Length < 16) 
					IV += IV; 
				IV = IV.Substring(0, 16); 

				byte[] key = StringToByteArray(Key); 
				byte[] iv = StringToByteArray(IV); 

				return Encode(Encrypt(Input, key, iv)); 
			} 
			catch(Exception) 
			{ 
				return ""; 
			} 
		} 

		public string DecryptString(string Input, string Key, string IV) 
		{ 
			try 
			{ 
				if(Key.Length == 0) 
					return ""; 
				while(Key.Length < this.KeySize) 
					Key += Key; 
				Key = Key.Substring(0, this.KeySize); 

				if(IV.Length == 0) 
					IV = Key; 
				while(IV.Length < 16) 
					IV += IV; 
				IV = IV.Substring(0, 16); 

				byte[] key = StringToByteArray(Key); 
				byte[] iv = StringToByteArray(IV); 

				return Decrypt(Decode(Input), key, iv); 
			} 
			catch(Exception) 
			{ 
				return ""; 
			} 
		} 

		private string Encrypt(string plaintext, byte[] key, byte[] initializationVector) 
		{ 

			string result = string.Empty; 
			try 
			{ 
				MemoryStream ms = new MemoryStream(); 
				CryptoStream encStream= new CryptoStream(ms, this.Algorithm.CreateEncryptor(key, initializationVector), CryptoStreamMode.Write); 
				encStream.Write(StringToByteArray(plaintext),0,plaintext.Length); 
				encStream.FlushFinalBlock(); 
				encStream.Flush(); 
				encStream.Close(); 
				result = MemoryStreamToString(ms); 
				ms.Close(); 
				return result; 
			} 
			catch(Exception) 
			{ 
				return result; 
			} 
		} 

		private string Decrypt(string plaintext, byte[] key, byte[] initializationVector) 
		{ 

			string result = string.Empty; 
			try 
			{ 
				MemoryStream ms = new MemoryStream(); 
				CryptoStream encStream = new CryptoStream(ms, this.Algorithm.CreateDecryptor(key, initializationVector), CryptoStreamMode.Write); 

				encStream.Write(StringToByteArray(plaintext), 0, plaintext.Length); 
				encStream.FlushFinalBlock(); 
				encStream.Flush(); 
				encStream.Close(); 
				result = MemoryStreamToString(ms); 
				ms.Close(); 

				return result; 
			} 
			catch(Exception) 
			{ 
				return ""; 
			} 
		} 

		private byte[] StringToByteArray(string s) 
		{ 
			byte[] result = new byte[s.Length]; 
			for(int i = 0; i < s.Length; i++) 
			{ 
				result[i]=(byte)s[i]; 
			} 
			return result; 
		} 

		private string MemoryStreamToString(MemoryStream source) 
		{ 
			string result = string.Empty; 
			result = ByteArrayToString(source.ToArray()); 
			return result; 
		} 

		private string ByteArrayToString(byte[] source) 
		{ 
			string result=string.Empty; 
			StringBuilder resultBuilder = new StringBuilder(source.Length); 
			foreach(byte b in source) 
			{ 
				resultBuilder.Append((char)b); 
			} 
			result = resultBuilder.ToString(); 
			return result; 
		} 
	}


	public class Symmetric 
	{
		static private Byte[] m_Key = new Byte[8]; 
		static private Byte[] m_IV = new Byte[8]; 
	
		//////////////////////////
		//Function to encrypt data
		public static string EncryptData(String strKey, String strData)
		{
			string strResult = String.Empty;		//Return Result

			//1. String Length cannot exceed 90Kb. Otherwise, buffer will overflow. See point 3 for reasons
			if (strData.Length > 92160)
			{
				strResult="Error. Data String too large. Keep within 90Kb.";
				return strResult;
			}
	
			//2. Generate the Keys
			if (!InitKey(strKey))
			{
				strResult="Error. Fail to generate key for encryption";
				return strResult;
			}

			//3. Prepare the String
			//	The first 5 character of the string is formatted to store the actual length of the data.
			//	This is the simplest way to remember to original length of the data, without resorting to complicated computations.
			//	If anyone figure a good way to 'remember' the original length to facilite the decryption without having to use additional function parameters, pls let me know.
			strData = String.Format("{0,5:00000}"+strData, strData.Length);


			//4. Encrypt the Data
			byte[] rbData = new byte[strData.Length];
			ASCIIEncoding aEnc = new ASCIIEncoding();
			aEnc.GetBytes(strData, 0, strData.Length, rbData, 0);
		
			DESCryptoServiceProvider descsp = new DESCryptoServiceProvider(); 
		
			ICryptoTransform desEncrypt = descsp.CreateEncryptor(m_Key, m_IV); 


			//5. Perpare the streams:
			//	mOut is the output stream. 
			//	mStream is the input stream.
			//	cs is the transformation stream.
			MemoryStream mStream = new MemoryStream(rbData); 
			CryptoStream cs = new CryptoStream(mStream, desEncrypt, CryptoStreamMode.Read);        
			MemoryStream mOut = new MemoryStream();
		
			//6. Start performing the encryption
			int bytesRead; 
			byte[] output = new byte[1024]; 
			do 
			{ 
				bytesRead = cs.Read(output,0,1024);
				if (bytesRead != 0) 
					mOut.Write(output,0,bytesRead); 
			} while (bytesRead > 0); 
		
			//7. Returns the encrypted result after it is base64 encoded
			//	In this case, the actual result is converted to base64 so that it can be transported over the HTTP protocol without deformation.
			if (mOut.Length == 0) 
			{	
				strResult = "";
			}
			else
			{
				//strResult = Convert.ToBase64String(mOut.GetBuffer(), 0, (int)mOut.Length);
				byte[] buffer = mOut.GetBuffer();
				int max = (int)mOut.Length;
				int count = 0;
				foreach (byte b in buffer) 
				{
					if (count < max)
						strResult+= b.ToString("X");
					else
						break;
					count++;
				}					
			}
	
			return strResult;
		}

		//////////////////////////
		//Function to decrypt data
		public static string DecryptData(String strKey, String strData)
		{
			string strResult;

			//1. Generate the Key used for decrypting
			if (!InitKey(strKey))
			{
				strResult="Error. Fail to generate key for decryption";
				return strResult;
			}

			//2. Initialize the service provider
			int nReturn = 0;
			DESCryptoServiceProvider descsp = new DESCryptoServiceProvider(); 
			ICryptoTransform desDecrypt = descsp.CreateDecryptor(m_Key, m_IV); 
		
			//3. Prepare the streams:
			//	mOut is the output stream. 
			//	cs is the transformation stream.
			MemoryStream mOut = new MemoryStream();
			CryptoStream cs = new CryptoStream(mOut, desDecrypt, CryptoStreamMode.Write);        
		
			//4. Remember to revert the base64 encoding into a byte array to restore the original encrypted data stream

			
			//strData = Convert.ToBase64String(



			byte[] bPlain = new byte[strData.Length];
			try 
			{
				//bPlain = Convert.FromBase64CharArray(strData.ToCharArray(), 0, strData.Length);
				bPlain = Dury.SiteFoundry.Utils.HexEncoding.GetBytes(strData);
			}
			catch (Exception) 
			{ 
				strResult = "Error. Input Data is not base64 encoded.";
				return strResult;
			}
		
			long lRead = 0;
			long lTotal = strData.Length;
		
			try
			{
				//5. Perform the actual decryption
				while (lTotal >= lRead)
				{ 
					cs.Write(bPlain,0,(int)bPlain.Length); 
					//descsp.BlockSize=64
					lRead = mOut.Length + Convert.ToUInt32(((bPlain.Length / descsp.BlockSize) * descsp.BlockSize));
				};

				
				ASCIIEncoding aEnc = new ASCIIEncoding();
				strResult = aEnc.GetString(mOut.GetBuffer(), 0, (int)mOut.Length);
			
				//6. Trim the string to return only the meaningful data
				//	Remember that in the encrypt function, the first 5 character holds the length of the actual data
				//	This is the simplest way to remember to original length of the data, without resorting to complicated computations.
				String strLen = strResult.Substring(0,5);
				int nLen = Convert.ToInt32(strLen);
				strResult = strResult.Substring(5, nLen);
				nReturn = (int)mOut.Length;
			
				return strResult;
			}
			catch (Exception)
			{
				strResult = "Error. Decryption Failed. Possibly due to incorrect Key or corrputed data";
				return strResult;
			}
		}

		/////////////////////////////////////////////////////////////
		//Private function to generate the keys into member variables
		static private bool InitKey(String strKey)
		{
			try
			{
				// Convert Key to byte array
				byte[] bp = new byte[strKey.Length];
				ASCIIEncoding aEnc = new ASCIIEncoding();
				aEnc.GetBytes(strKey, 0, strKey.Length, bp, 0);
			
				//Hash the key using SHA1
				SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
				byte[] bpHash = sha.ComputeHash(bp);
			
				int i;
				// use the low 64-bits for the key value
				for (i=0; i<8; i++) 
					m_Key[i] = bpHash[i];
				
				for (i=8; i<16; i++) 
					m_IV[i-8] = bpHash[i];
			
				return true;
			}
			catch (Exception)
			{
				//Error Performing Operations
				return false;
			}
		}
	}




}
