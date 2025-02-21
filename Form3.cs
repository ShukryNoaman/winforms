﻿using System;
using System.Numerics;
using System.Windows.Forms;

namespace RSA_algorithm
{
    public  partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

      
        bool flag = true;
      static  BigInteger p = 0, q = 0, d = 0;
        long t = 0;
        DateTime Time = DateTime.Now;

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                String s = textBox1.Text.ToString();
                listBox1.Items.Add("digit=" + s.ToString());

                listBox1.Items.Add("length=" + s.Length);
                BigInteger n = BigInteger.Parse(s);
                listBox1.Items.Add("n=" + n);
                q = Sqrt(n) + 1;
                d = q;
                flag = true;
                long t = DateTime.Now.ToFileTime();
                //MessageBox.Show(q + "                 ee   " + n);
                listBox1.Items.Add("q=" + q.ToString());
                // listBox1.Items.Add("length of q=" + q.ToString().Length);
                //  BigInteger aa = BigInteger.Multiply(q, q);
                //listBox1.Items.Add("aa=" + aa);
                int l = 0;
                // t = Time.ToLongTimeString().;// Convert.ToInt64(Time.ToLongTimeString());
                while (flag)
                {
                    if (((q % 5) == 0) && ((q % 2) != 0))
                        flag = false;
                    else q = q + 1;
                    listBox1.Items.Add("first loop step=" + l + " \t q=" + q.ToString());
                    l++;
                }
                flag = true;
                l = 0;
                while (flag)
                {
                    q = q - 2;
                    for (int i = 0; i <= 3; i++)
                    {
                        if ((n % q) == 0)
                        {
                            p = n / q;
                            flag = false;
                            break;
                        }
                        else q = q - 2;
                        l++;
                        listBox1.Items.Add("second loop step=" + l + "\t q=" + q);
                    }
                }

                long t1 = DateTime.Now.ToFileTime() - t;
                listBox2.Items.Add("n=" + n);
                listBox2.Items.Add("q=" + d);
                listBox2.Items.Add("p=" + p);
                listBox2.Items.Add("t=" + t1 + "  milliseconds");
                listBox2.Items.Add("t=" + t1 / 1000 + "  seconds");
            }
            catch (Exception ee) { MessageBox.Show(ee.Message); }
        }


        /*  BigInteger RoughRoot(BigInteger x, int root)
         {
             var bytes = x.ToByteArray();    // get binary representation
             var bits = (bytes.Length - 1) * 8;  // get # bits in all but msb
                                                 // add # bits in msb
             for (var msb = bytes[bytes.Length - 1]; msb != 0; msb >>= 1)
                 bits++;
             var rtBits = bits / root + 1;   // # bits in the root
             var rtBytes = rtBits / 8 + 1;   // # bytes in the root
                                             // avoid making a negative number by adding an extra 0-byte if the high bit is set
             var rtArray = new byte[rtBytes + (rtBits % 8 == 7 ? 1 : 0)];
             // set the msb
             rtArray[rtBytes - 1] = (byte)(1 << (rtBits % 8));
             // make new BigInteger from array of bytes
             return new BigInteger(rtArray);
         }*/

        /*
        public  BigInteger IntegerRoot(BigInteger n, int root)
        {
            var oldValue = new BigInteger(0);
            var newValue = RoughRoot(n, root);
            int i = 0;
            // I limited iterations to 100, but you may want way less
            while (BigInteger.Abs(newValue - oldValue) >= 1 && i < 100)
            {
                oldValue = newValue;
                newValue = ((root - 1) * oldValue
                            + (n / BigInteger.Pow(oldValue, root - 1))) / root;
                i++;
            }
            return newValue;
        }*/
        BigInteger RoughSqrt(BigInteger x)
        {
            var bytes = x.ToByteArray();    // get binary representation
            var bits = (bytes.Length - 1) * 8;  // get # bits in all but msb
                                                // add # bits in msb
            for (var msb = bytes[bytes.Length - 1]; msb != 0; msb >>= 1)
                bits++;
            var sqrtBits = bits / 2;    // # bits in the sqrt
            var sqrtBytes = sqrtBits / 8 + 1;   // # bytes in the sqrt
                                                // avoid making a negative number by adding an extra 0-byte if the high bit is set
            var sqrtArray = new byte[sqrtBytes + (sqrtBits % 8 == 7 ? 1 : 0)];
            // set the msb
            sqrtArray[sqrtBytes - 1] = (byte)(1 << (sqrtBits % 8));
            // make new BigInteger from array of bytes
            return new BigInteger(sqrtArray);
        }
        BigInteger RoughRoot(BigInteger x, int root)
        {
            var bytes = x.ToByteArray();    // get binary representation
            var bits = (bytes.Length - 1) * 8;  // get # bits in all but msb
                                                // add # bits in msb
            for (var msb = bytes[bytes.Length - 1]; msb != 0; msb >>= 1)
                bits++;
            var rtBits = bits / root + 1;   // # bits in the root
            var rtBytes = rtBits / 8 + 1;   // # bytes in the root
                                            // avoid making a negative number by adding an extra 0-byte if the high bit is set
            var rtArray = new byte[rtBytes + (rtBits % 8 == 7 ? 1 : 0)];
            // set the msb
            rtArray[rtBytes - 1] = (byte)(1 << (rtBits % 8));
            // make new BigInteger from array of bytes
            return new BigInteger(rtArray);
        }
        public BigInteger IntegerRoot(BigInteger n, int root)
        {
            var oldValue = new BigInteger(0);
            var newValue = RoughRoot(n, root);
            int i = 0;
            // I limited iterations to 100, but you may want way less
            while (BigInteger.Abs(newValue - oldValue) >= 1 && i < 100)
            {
                oldValue = newValue;
                newValue = ((root - 1) * oldValue
                            + (n / BigInteger.Pow(oldValue, root - 1))) / root;
                i++;
            }
            return newValue;
        }
        //99999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999100000000000000001097906362944045541740492309677311846336810682903157585404911491537163328978494688899061249669721172515611590283743140088328307009198146046031271664502933027185697489699588559043338384466165001178426897626212945177628091195786707458122783970171784415105291802893207873272974885715430223118336
        //99999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999100000000000000001097906362944045541740492309677311846336810682903157585404911491537163328978494688899061249669721172515611590283743140088328307009198146046031271664502933027185697489699588559043338384466165001178426897626212945177628091195786707458122783970171784415105291802893207873272974885715430223118336
        //179769313486231570814527423731704356798070567525844996598917476803157260780028538760589558632766878171540458953514382464234321326889464182768467546703537516986049910576551282076245490090389328944075868508455133942304583236903222948165808559332123348274797826204144723168738177180919299881250404026184124858368

        //30576148829740138953999438154435898092818919944533907506328558978559133324120499819858510061160544485423038309265225641262711922181710849419227945301717887409045108401198337094223287364797822419556232895905120129780873950688569433634916048421351767823038918734482252396717444751003454441061564466523259364390514017386934600569265634819534200864654170662400560775759617256148235309605883714640560228789958485809590774593681062035669045090437411568745546591963643442240406835241222081558413063318123260026420089227327133756040905398490440352160605856566874897425839282123404657432219978117
        //101017638707436133903821306341466727228541580658758890103412581005475252078199915929932968020619524277851873319243238741901729414629681623307196829081607677830881341203504364437688722228526603134919021724454060938836833023076773093013126674662502999661052433082827512395099052335602854935571690613335742455727
        public BigInteger IntegerSqrt(BigInteger n)
        {
            var oldValue = new BigInteger(0);
            BigInteger newValue = RoughSqrt(n);
            int i = 0;
            while (BigInteger.Abs(newValue - oldValue) >= 1)
            {
                oldValue = newValue;
                newValue = (oldValue + (n / oldValue)) / 2;
                i++;
            }
            return newValue;
        }

        public BigInteger Sqrt(BigInteger n)
        {
            if (n == 0) return 0;
            if (n > 0)
            {
                int bitLength = Convert.ToInt32(Math.Ceiling(BigInteger.Log(n, 2)));
                BigInteger root = BigInteger.One << (bitLength / 2);

                while (!isSqrt(n, root))
                {
                    root += n / root;
                    root /= 2;
                }

                return root;
            }

            throw new ArithmeticException("NaN");
        }

        private Boolean isSqrt(BigInteger n, BigInteger root)
        {
            BigInteger lowerBound = root * root;
            BigInteger upperBound = (root + 1) * (root + 1);

            return (n >= lowerBound && n < upperBound);
        }
    }
}
