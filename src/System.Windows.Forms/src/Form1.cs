﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Numerics;
using System.Diagnostics;
using System.Windows.Forms;

namespace RSA_algorithm
{

    public partial class Form1 : Form
    {
        bool flag = true;
        BigInteger p = 0, q = 0,d=0;
        long t = 0;
        DateTime Time = DateTime.Now;


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        /*  public static BigInteger Sqrt(this BigInteger number)
          {
              BigInteger n = 0, p = 0;
              if (number == BigInteger.Zero)
              {
                  return BigInteger.Zero;
              }
              var high = number >> 1;
              var low = BigInteger.Zero;

              while (high > low + 1)
              {
                  n = (high + low) >> 1;
                  p = n * n;
                  if (number == p)
                  {
                      high = n;
                  }
                  else if (number > p)
                  {
                      low = n;
                  }
                  else
                  {
                      break;
                  }
              }
              return number == p ? n : low;
          }

      */
        static BigInteger RoughRoot(BigInteger x, int root)
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

        public static BigInteger IntegerRoot(BigInteger n, int root)
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
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                String s = textBox1.Text.ToString();
                listBox1.Items.Add("digit="+s.ToString());

                listBox1.Items.Add("length="+s.Length);
                BigInteger n = BigInteger.Parse(s);
                listBox1.Items.Add("n=" + n);
                q = IntegerRoot(n,2)+1;
                d = q;            
                flag = true;
                long t =DateTime.Now.ToFileTime();
               //MessageBox.Show(q + "                 ee   " + n);
                listBox1.Items.Add("q="+q.ToString());
               // listBox1.Items.Add("length of q=" + q.ToString().Length);
              //  BigInteger aa = BigInteger.Multiply(q, q);
//listBox1.Items.Add("aa=" + aa);
                int l = 0;
                // t = Time.ToLongTimeString().;// Convert.ToInt64(Time.ToLongTimeString());
              while (flag) {
                    if (((q%5)==0) && ((q%2)!=0))
                        flag = false;
                    else q=q+1;
                  listBox1.Items.Add("first loop step="+l+" \t q="+q.ToString());
                    l++;
                }
             flag = true;
                l = 0;
               while (flag) {
                    q=q-2;
                   for (int i=0;i<=3;i++) {
                  if ((n%q)==0)
                        {
                            p=n/q;
                            flag = false;
                            break;
                        }
                        else q=q-2;
                        l++;
                      listBox1.Items.Add("second loop step="+l+"\t q=" + q);
                    }
               }

                long t1 =  DateTime.Now.ToFileTime()-t;
                listBox2.Items.Add("n=" + n);
                listBox2.Items.Add("q=" + d);
                listBox2.Items.Add("p=" + p);
                listBox2.Items.Add("t=" + t1+ "  milliseconds"); 
            }
            catch (Exception ee) { MessageBox.Show(ee.Message); }
        }
    }
}
