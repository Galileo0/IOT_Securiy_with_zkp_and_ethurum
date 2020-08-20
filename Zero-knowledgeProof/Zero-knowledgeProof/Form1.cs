﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Numerics;

namespace Zero_knowledgeProof
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            long V, N;
            double S;
            ZEROPROOF.GenerateKeys(out V, out S, out N);
            textBoxAN.Text = Convert.ToString(N);
            textBoxAV.Text = Convert.ToString(V);
            textBoxBN.Text = Convert.ToString(N);
            textBoxBV.Text = Convert.ToString(V);
            textBoxAS.Text = Convert.ToString(S);
        }

        private void buttonIdentif_Click(object sender, EventArgs e)
        {
            textBoxLog.Clear();
            textBoxLog.AppendText("=====================" + Environment.NewLine);
            textBoxLog.AppendText("Check Start" + Environment.NewLine);
            textBoxLog.AppendText("=====================" + Environment.NewLine);
            var Rnd = new Random();
            var Proof = true;

            var AN = Convert.ToInt64(textBoxAN.Text);
            var AS = Convert.ToInt64(textBoxAS.Text);
            var BN = Convert.ToInt64(textBoxBN.Text);
            var BV = Convert.ToInt64(textBoxBV.Text);

            var NeBudetPovtornoR = new HashSet<int>();
            for (var i = 1; i<=1; i++)
            {
                textBoxLog.AppendText("---------------------" + Environment.NewLine);
                textBoxLog.AppendText("Side A(proves)" + Environment.NewLine);
                textBoxLog.AppendText("---------------------" + Environment.NewLine);

                var AR = 0;
                do
                {
                    AR = Rnd.Next(1, (int)AN);
                }
                while (NeBudetPovtornoR.Contains(AR));
                NeBudetPovtornoR.Add(AR);
                textBoxLog.AppendText("Random R = " + Convert.ToString(AR) + Environment.NewLine);

                var AX = ZEROPROOF.FastPowFunc(AR,2, AN);
                textBoxLog.AppendText("X = " + Convert.ToString(AX) + Environment.NewLine);

                textBoxLog.AppendText("Sending to X side B" + Environment.NewLine);

                //---------------------------------------------

                textBoxLog.AppendText("---------------------" + Environment.NewLine);
                textBoxLog.AppendText("Side B(verifies)" + Environment.NewLine);
                textBoxLog.AppendText("---------------------" + Environment.NewLine);

                textBoxLog.AppendText("Getting X from the side A" + Environment.NewLine);
                var BX = AX;
                textBoxLog.AppendText("X = " + Convert.ToString(BX) + Environment.NewLine);

                var Bb = Rnd.Next(0, 2);
                textBoxLog.AppendText("Random bit b = " + Convert.ToString(Bb) + Environment.NewLine);

                textBoxLog.AppendText("Sending to b side A" + Environment.NewLine);

                //---------------------------------------------

                textBoxLog.AppendText("---------------------" + Environment.NewLine);
                textBoxLog.AppendText("Side A(proves)" + Environment.NewLine);
                textBoxLog.AppendText("---------------------" + Environment.NewLine);

                textBoxLog.AppendText("Getting b from the side B" + Environment.NewLine);
                var Ab = Bb;
                textBoxLog.AppendText("b = " + Convert.ToString(Ab) + Environment.NewLine);

                long AY = 0;
                if (Ab == 0)
                    textBoxLog.AppendText("Sending to R side B" + Environment.NewLine);
                else
                {
                   AY = AR * AS % AN;
                    textBoxLog.AppendText("Y = " + Convert.ToString(AY) + Environment.NewLine);
                    textBoxLog.AppendText("Sending to Y side B" + Environment.NewLine);
                }

                //---------------------------------------------

                textBoxLog.AppendText("---------------------" + Environment.NewLine);
                textBoxLog.AppendText("Side B(verifies)" + Environment.NewLine);
                textBoxLog.AppendText("---------------------" + Environment.NewLine);

                if (Bb == 0)
                {
                    textBoxLog.AppendText("Getting R from the side A" + Environment.NewLine);
                    var BR = AR;
                    textBoxLog.AppendText("R = " + Convert.ToString(BR) + Environment.NewLine);
                    textBoxLog.AppendText("Calculated X = " + Convert.ToString(ZEROPROOF.FastPowFunc(BR, 2, BN)) + Environment.NewLine);
                    textBoxLog.AppendText("Received X = " + Convert.ToString(BX) + Environment.NewLine);
                    if (BX == ZEROPROOF.FastPowFunc(BR, 2, BN))
                        textBoxLog.AppendText("Party A knows sqrt(X)" + Environment.NewLine);
                    else
                    {
                        textBoxLog.AppendText("Side A does not know sqrt(X).Party A is not genuine!" + Environment.NewLine);
                        Proof = false;
                        break;
                    }
                }
                else
                {
                    textBoxLog.AppendText("Getting Y from the side A" + Environment.NewLine);
                    var BY = AY;
                    textBoxLog.AppendText("Y = " + Convert.ToString(BY) + Environment.NewLine);
                    textBoxLog.AppendText("Calculated X = " + (BigInteger.Pow(BY, 2) * BV % BN).ToString() + Environment.NewLine);
                    textBoxLog.AppendText("Received X = " + Convert.ToString(BX) + Environment.NewLine);
                    if (BX == Convert.ToInt64((BigInteger.Pow(BY, 2) * BV % BN).ToString()))
                        textBoxLog.AppendText("Party A knows sqrt(V-1)" + Environment.NewLine);
                    else
                    {
                        textBoxLog.AppendText("Side A does not know sqrt (V-1). Side A is not genuine!" + Environment.NewLine);
                        Proof = false;
                        break;
                    }
                }

            }
            if (Proof)
                textBoxLog.AppendText("Party A identified!" + Environment.NewLine);
            else
                textBoxLog.AppendText("Party A not identified!" + Environment.NewLine);
        }
    }

    static public class ZEROPROOF
    {

        private static List<int> ReshetoEratosfena(out int P, out int Q)
        {

            var IntArr = new List<int>();
            for (var i = 2; i <= 1000; i++)
                IntArr.Add(i);

            for (var i = 0; i < IntArr.Count; i++)
            {
                for (var j = i; j < IntArr.Count; j++)
                {
                    if ((IntArr[i] != IntArr[j]) && (IntArr[j] % IntArr[i] == 0))
                    {
                        IntArr.RemoveAt(j);
                        j--;
                    }
                }
            }

            var ResList = new List<int>(IntArr);

            for (var i = 0; i < IntArr.Count;)
                if (IntArr[i] < 100)
                    IntArr.Remove(IntArr[i]);
                else
                    break;

            P = IntArr[new Random().Next(0, IntArr.Count - 1)];
            do
            {
                Q = IntArr[new Random().Next(0, IntArr.Count - 1)];
            }
            while (Q == P);

            return ResList;
        }

        private static bool EuclidIsNOD1(long a, long b)
        {

            while (a != 0 && b != 0)
            {
                if (a > b)
                    a -= b;
                else
                    b -= a;
            }
            if (a == 1 || b == 1)
                return true;
            else
                return false;
        }

        static public Int64 FastPowFunc(Int64 Number, Int64 Pow, Int64 Mod)
        {
            Int64 Result = 1;
            Int64 Bit = Number % Mod;

            while (Pow > 0)
            {
                if ((Pow & 1) == 1)
                {
                    Result *= Bit;
                    Result %= Mod;
                }
                Bit *= Bit;
                Bit %= Mod;
                Pow >>= 1;
            }
            return Result;
        }

        static public void GenerateKeys(out long V, out double S, out long N)
        {
            int P, Q;
            SortedSet<long> Vi;
            do
            {
                ReshetoEratosfena(out P, out Q);
                N = P * Q;

                Vi = new SortedSet<long>();
                long X = 1;
                do
                {
                    X++;
                    if (EuclidIsNOD1((long)Math.Pow(X, 2) % N, N))
                        Vi.Add((long)Math.Pow(X, 2) % N);
                }
                while (X < N);
            }
            while (Vi.Count == 1);
            long Vminus1 = 0;
            long Tmp;
            var Rnd = new Random();
            var M = (P - 1) * (Q - 1);
            do
            {
                V = 1;
                while (V == 1)
                    V = Vi.ToArray()[Rnd.Next(0, Vi.Count)];
                Vminus1 = FastPowFunc(V, M - 1, N);
            }
            while (!long.TryParse(Convert.ToString(Math.Sqrt(Vminus1)), out Tmp));
            S = Math.Sqrt(Vminus1) % N;
        }

    }
}
