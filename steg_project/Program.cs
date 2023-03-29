
using System;
using System.IO;
using System.Collections;
class Steganography{


  public static byte[] GetInput(string[] args)
  {
    string input = "";

    if(args.Length == 1)
    {
      input = args[0];
    } 
    else
    {
      Console.WriteLine("No inputs found!");
    }

    string[] strBytes = input.Split(" ");

    byte[] inputBytes = new byte[strBytes.Length];

    for(int i = 0; i < strBytes.Length; i++)
    {
      inputBytes[i] = Convert.ToByte(strBytes[i],16);
    }
    return inputBytes;
  }

  public static byte[] ProcessInput(byte[] input, byte[] bmpBytes)
  {

    // Using BitArray Manages a compact array of bit values, which are represented as Booleans, where true indicates that the bit is on (1) and false indicates the bit is off (0).
    BitArray inputBits = new BitArray(input);

    byte[] result = new byte[bmpBytes.Length];
    
    // Keep the first 26 bytes as is
    for(int i = 0; i < 26; i++)
    {
      result[i] = bmpBytes[i];
    }
    // For the rest
    for(int i = 26; i < bmpBytes.Length; i++)
    {
      int indexOffset = i - 26;
      int leftBit = (indexOffset/4)*8 + (7-2*(indexOffset%4));
      int rightBit = leftBit - 1;
      byte xorByte = (byte)((Convert.ToByte(inputBits[leftBit]) << 1) | Convert.ToByte(inputBits[rightBit]));
  
      result[i] = (byte)(bmpBytes[i] ^ xorByte);
    }



    return result;
  }
  static void Main(string[] args)
  {

 // Blue pixel = 0xFF,0x00,0x00
// Red pixel = 0x00,0x00,0xFF
// White pixel = 0xFF,0xFF,0xFF
// Black pixel = 0x00,0x00,0x00

    byte[] bmpBytes = new byte[] {
    0x42,0x4D,0x4C,0x00,0x00,0x00,0x00,0x00,
    0x00,0x00,0x1A,0x00,0x00,0x00,0x0C,0x00,
    0x00,0x00,0x04,0x00,0x04,0x00,0x01,0x00,
    0x18,0x00,
    0x00,0x00,0xFF,0xFF,0xFF,0xFF,
    0x00,0x00,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,
    0xFF,0x00,0x00,0x00,0xFF,0xFF,0xFF,0x00,
    0x00,0x00,0xFF,0x00,0x00,0xFF,0xFF,0xFF,
    0xFF,0x00,0x00,0xFF,0xFF,0xFF,0xFF,0xFF,
    0xFF,0x00,0x00,0x00,0xFF,0xFF,0xFF,0x00,
    0x00,0x00
    };
    
    // string test = "B1 FF FF CC 98 80 09 EA 04 48 7E C9";

    byte[] inputBytes = GetInput(args);
    byte[] resultBmp = ProcessInput(inputBytes, bmpBytes);
    Console.WriteLine(BitConverter.ToString(resultBmp).Replace("-"," "));
    // Console.WriteLine(BitConverter.ToString(bmpBytes).Replace("-", " "));
  } 
}