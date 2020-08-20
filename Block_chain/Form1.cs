using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nethereum.Web3;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Util;
using Nethereum.ABI;


namespace Block_chain
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        Web3 web3 = new Nethereum.Web3.Web3();
        
        public string balacne;

        public void connect_to_rpc(string url)
        {   // This Function Used to Connect to local network (RPC)
            web3 = new Nethereum.Web3.Web3(url);
            listView1.Items.Add("RPC -> Connected");
           
        }
        private void button7_Click(object sender, EventArgs e)
        {
            connect_to_rpc(textBox1.Text);
        }

        public async void Get_balance()
        {
            // test function only
            var web3 = new Nethereum.Web3.Web3("http://127.0.0.1:8545");
            var g =  await web3.Eth.GetBalance.SendRequestAsync("0x392ca5365cd9466f7932a1ccE32c6bd180848e8E").ConfigureAwait(false) ;
            balacne = g.ToString();
        }

        public async void list_accounts()
        {

        }

        private async void button8_Click(object sender, EventArgs e)
        {
            // Test Function only
            var accounts = await web3.Personal.ListAccounts.SendRequestAsync().ConfigureAwait(false);
            foreach(var x in accounts)
            {
                string acc = x.ToString();
                if (listView1.InvokeRequired)
                {
                    listView1.Invoke((MethodInvoker)delegate ()
                    {
                        listView1.Items.Add(x);
                    });
                }
            }
            
                
        }

        public async void contract()
        {
            // test contract function ony
            var abi = textBox2.Text;
            var address = textBox3.Text;
            var con = web3.Eth.GetContract(abi, address);
            var test_function = con.GetFunction("set_values");
           // var result = await test_function.CallAsync<string>("aa","ba","ca","da");
            var result = await test_function.CallAsync<int>("a","b","c","d");

            if (result == 1)
            {
                listView1.Items.Add("Successs");
            }
            else
            {
                listView1.Items.Add("failedddd");
            }
        }

        public async void set_key1()
        {
            var abi = textBox2.Text;    // get abi
            var address = textBox3.Text; // get contract address 
            var con = web3.Eth.GetContract(abi, address); // call contract
            var function = con.GetFunction("set_sensor_data"); // select function from contract (solidty)
            //generate keys by zero knowldge prof
            long V, N;   // keys
            double S;
            ZEROPROOF.GenerateKeys(out V, out S, out N); // generate keys
            var result = await function.CallAsync<int>("Hurt_Rate", V.ToString(),S.ToString(),N.ToString()); // call contract funtion
            //account choose 
            var accounts = await web3.Personal.ListAccounts.SendRequestAsync().ConfigureAwait(false); // select account to make transaction 
            var test_sender = "";
            foreach (var x in accounts)
            {
                test_sender = x.ToString();
                break;  // break to take one account
            }

            // test encode

            var abiencode = new ABIEncode();  // function used to encode transaction 

             
            var gas = await function.EstimateGasAsync("temp", V.ToString(), S.ToString(), N.ToString()).ConfigureAwait(false); //calculate gas need to make transation
            //var trans = await function.SendTransactionAsync(test_sender, "temp", V.ToString(), S.ToString(), N.ToString());
            var receipt7 = await function.SendTransactionAndWaitForReceiptAsync(test_sender, gas,null,null, "temp", V.ToString(), S.ToString(), N.ToString()).ConfigureAwait(false); // make transaction
            var test_encode = abiencode.GetABIEncoded(receipt7).ToHex();
            var test_encode2 = abiencode.GetSha3ABIEncodedPacked(receipt7).ToHex(); // encode transaction
            if (result == 1)
            {
                if (listView1.InvokeRequired) // display results 
                {
                    listView1.Invoke((MethodInvoker)delegate ()
                    {
                        listView1.Items.Add("Sensor 1 Set Keys");
                        listView1.Items.Add(V.ToString());
                        listView1.Items.Add(S.ToString());
                        listView1.Items.Add(N.ToString());
                        listView1.Items.Add(test_encode.ToString());
                        listView1.Items.Add(test_encode2.ToString());
                    });
                }
                
            }
            else
            {
                listView1.Items.Add("Sensor 1 set feild");
            }

        }

        public async void set_key2()
        {
            var abi = textBox2.Text;
            var address = textBox3.Text;
            var con = web3.Eth.GetContract(abi, address);
            var function = con.GetFunction("set_sensor_data");
            //generate keys by zero knowldge prof
            long V, N;
            double S;
            ZEROPROOF.GenerateKeys(out V, out S, out N);
            var result = await function.CallAsync<int>("temp", V.ToString(), S.ToString(), N.ToString());
            //account choose 
            var accounts = await web3.Personal.ListAccounts.SendRequestAsync().ConfigureAwait(false);
            var test_sender = "";
            foreach (var x in accounts)
            {
                test_sender = x.ToString();
                break;
            }

            var gas = await function.EstimateGasAsync("temp", V.ToString(), S.ToString(), N.ToString()).ConfigureAwait(false);
            //var trans = await function.SendTransactionAsync(test_sender, "temp", V.ToString(), S.ToString(), N.ToString());
            var receipt7 = await function.SendTransactionAndWaitForReceiptAsync(test_sender, gas, null, null, "temp", V.ToString(), S.ToString(), N.ToString()).ConfigureAwait(false);
            var abiencode = new ABIEncode();
            var test_encode = abiencode.GetABIEncoded(receipt7).ToHex();
            var test_encode2 = abiencode.GetSha3ABIEncodedPacked(receipt7).ToHex();
            if (result == 1)
            {
                if (listView1.InvokeRequired)
                {
                    listView1.Invoke((MethodInvoker)delegate ()
                    {
                        listView1.Items.Add("Sensor 2 Set Keys");
                        listView1.Items.Add(V.ToString());
                        listView1.Items.Add(S.ToString());
                        listView1.Items.Add(N.ToString());
                    });
                }
                
            }
            else
            {
                listView1.Items.Add("Sensor 2 set feild");
            }

        }

        public async void set_key2_fake()
        {
            var abi = textBox2.Text;
            var address = textBox3.Text;
            var con = web3.Eth.GetContract(abi, address);
            var function = con.GetFunction("update_sensor_data");
            //generate keys by zero knowldge prof
            long V, N;
            double S;
            Random rand = new Random();
            V = rand.Next();
            N = rand.Next();
            S = rand.Next();
            var result = await function.CallAsync<int>("temp", V.ToString(), S.ToString(), N.ToString());
            var accounts = await web3.Personal.ListAccounts.SendRequestAsync().ConfigureAwait(false);
            var test_sender = "";
            foreach (var x in accounts)
            {
                test_sender = x.ToString();
                break;
            }

            var gas = await function.EstimateGasAsync("temp", V.ToString(), S.ToString(), N.ToString()).ConfigureAwait(false);
            //var trans = await function.SendTransactionAsync(test_sender, "temp", V.ToString(), S.ToString(), N.ToString());
            var receipt7 = await function.SendTransactionAndWaitForReceiptAsync(test_sender, gas, null, null, "temp", V.ToString(), S.ToString(), N.ToString()).ConfigureAwait(false);
            var abiencode = new ABIEncode();
            var test_encode = abiencode.GetABIEncoded(receipt7).ToHex();
            var test_encode2 = abiencode.GetSha3ABIEncodedPacked(receipt7).ToHex();
            if (result == 1)
            {
                if (listView1.InvokeRequired)
                {
                    listView1.Invoke((MethodInvoker)delegate ()
                    {
                        listView1.Items.Add("Sensor 2 update Keys");
                    });
                }

                
            }
            else
            {
                listView1.Items.Add("Sensor 2 update feild");
            }

        }

        public async void set_key1_fake()
        {
            // this funtion used to update sensor zkp keys with fake random keys
            var abi = textBox2.Text;// get abi
            var address = textBox3.Text; // get contract address
            var con = web3.Eth.GetContract(abi, address);// call contract
            var function = con.GetFunction("update_sensor_data"); // select function from contract (solidty)
            
            long V, N; // fake keys
            double S;
            Random rand = new Random();  // random to create random keys
            V = rand.Next();
            N = rand.Next();
            S = rand.Next();
            var result = await function.CallAsync<int>("Hurt_Rate", V.ToString(), S.ToString(), N.ToString());// call contract funtion
            var accounts = await web3.Personal.ListAccounts.SendRequestAsync().ConfigureAwait(false);// select account to make transaction
            var test_sender = "";
            foreach (var x in accounts)
            {
                test_sender = x.ToString(); // break to take one account
                break;
            }

            var gas = await function.EstimateGasAsync("Hurt_Rate", V.ToString(), S.ToString(), N.ToString()).ConfigureAwait(false);//calculate gas need to make transation
            //var trans = await function.SendTransactionAsync(test_sender, "temp", V.ToString(), S.ToString(), N.ToString());
            var receipt7 = await function.SendTransactionAndWaitForReceiptAsync(test_sender, gas, null, null, "Hurt_Rate", V.ToString(), S.ToString(), N.ToString()).ConfigureAwait(false);// make transaction
            var abiencode = new ABIEncode();
            var test_encode = abiencode.GetABIEncoded(receipt7).ToHex();
            var test_encode2 = abiencode.GetSha3ABIEncodedPacked(receipt7).ToHex();  // encode transaction
            if (result == 1)
            {
                if (listView1.InvokeRequired)
                {
                    listView1.Invoke((MethodInvoker)delegate ()
                    {
                        listView1.Items.Add("Sensor 2 update Keys"); // display results
                    });
                }
                
            }
            else
            {
                listView1.Items.Add("Sensor 2 update feild");
            }

        }

        public async void set_data_1()
        {

            // this function used to simulate a senesor as reading values and send it as transaction
            var watch = new System.Diagnostics.Stopwatch(); // used to calcuate time
            watch.Start(); // start calculate
            // get keys for sensor1
            var abi = textBox2.Text; // get contract abi
            var address = textBox3.Text; // get contract adress
            var con = web3.Eth.GetContract(abi, address); // connect to contract
            var function = con.GetFunction("get_sensor_key_V"); // call functions to get sensor keys from contract 
            var _V = await function.CallAsync<string>("Hurt_Rate");
            function = con.GetFunction("get_sensor_key_S");
            var _S = await function.CallAsync<string>("Hurt_Rate");
            function = con.GetFunction("get_sensor_key_N");
            var _N = await function.CallAsync<string>("Hurt_Rate");
            listView1.Items.Add(_V);
            listView1.Items.Add(_S);
            listView1.Items.Add(_N);    // display keys

            int proof = zkp_verfy_no_print(_V, _S, _N); // verfy keys 
            if(proof == 1) 
            {
                // verfyed 
                // allow sensor to set values
                Random rand = new Random();  // generate values
                
                string sensor_name = "Hurt_Rate";
                int rate = rand.Next();
                DateTime now = DateTime.Now;
                function = con.GetFunction("set_reads"); // call function from contract to set this values 
                var result = await function.CallAsync<int>(sensor_name.ToString(), rate.ToString(),now.ToString()); ;
                var accounts = await web3.Personal.ListAccounts.SendRequestAsync().ConfigureAwait(false); // select account
                var test_sender = "";
                foreach (var x in accounts)
                {
                    test_sender = x.ToString();
                    break;
                }

                var gas = await function.EstimateGasAsync(sensor_name.ToString(), rate.ToString(), now.ToString()).ConfigureAwait(false); //calcuate gas required to make transaction
                //var trans = await function.SendTransactionAsync(test_sender, "temp", V.ToString(), S.ToString(), N.ToString());
                var receipt7 = await function.SendTransactionAndWaitForReceiptAsync(test_sender, gas, null, null, sensor_name.ToString(), rate.ToString(), now.ToString()).ConfigureAwait(false); // make transction
                var abiencode = new ABIEncode();
                var test_encode = abiencode.GetABIEncoded(receipt7).ToHex();
                var test_encode2 = abiencode.GetSha3ABIEncodedPacked(receipt7).ToHex(); // encode transction
                if (result == 1)
                {
                    if (listView1.InvokeRequired)
                    {
                        listView1.Invoke((MethodInvoker)delegate ()
                        {
                            listView1.Items.Add("Sensors Setting values");
                            listView1.Items.Add(sensor_name.ToString());
                            listView1.Items.Add(rate.ToString()); // display results
                            
                            listView1.Items.Add(now.ToString());
                        });
                    }

                }
                else
                {
                    listView1.Items.Add("Sensors set values feild");
                }

            

        }
            watch.Stop(); // stop calculate time
            if (textBox4.InvokeRequired)
            {
                textBox4.Invoke((MethodInvoker)delegate ()
                {
                    textBox4.Text = watch.ElapsedMilliseconds.ToString() + " MS";  // display benchmark
                });
            }
        }

        public async void set_data_2()
        {

            // this function used to simulate a senesor as reading values and send it as transaction
            var watch = new System.Diagnostics.Stopwatch(); // used to calcuate time
            watch.Start(); // start calculate
            // get keys for sensor1
            var abi = textBox2.Text; // get contract abi
            var address = textBox3.Text; // get contract adress
            var con = web3.Eth.GetContract(abi, address); // connect to contract
            var function = con.GetFunction("get_sensor_key_V"); // call functions to get sensor keys from contract 
            var _V = await function.CallAsync<string>("temp");
            function = con.GetFunction("get_sensor_key_S");
            var _S = await function.CallAsync<string>("temp");
            function = con.GetFunction("get_sensor_key_N");
            var _N = await function.CallAsync<string>("temp");
            listView1.Items.Add(_V);
            listView1.Items.Add(_S);
            listView1.Items.Add(_N);    // display keys

            int proof = zkp_verfy_no_print(_V, _S, _N); // verfy keys 
            if (proof == 1)
            {
                // verfyed 
                // allow sensor to set values
                Random rand = new Random();  // generate values

                string sensor_name = "temp";
                int rate = rand.Next();
                DateTime now = DateTime.Now;
                function = con.GetFunction("set_reads"); // call function from contract to set this values 
                var result = await function.CallAsync<int>(sensor_name.ToString(), rate.ToString(), now.ToString()); ;
                var accounts = await web3.Personal.ListAccounts.SendRequestAsync().ConfigureAwait(false); // select account
                var test_sender = "";
                foreach (var x in accounts)
                {
                    test_sender = x.ToString();
                    break;
                }

                var gas = await function.EstimateGasAsync(sensor_name.ToString(), rate.ToString(), now.ToString()).ConfigureAwait(false); //calcuate gas required to make transaction
                //var trans = await function.SendTransactionAsync(test_sender, "temp", V.ToString(), S.ToString(), N.ToString());
                var receipt7 = await function.SendTransactionAndWaitForReceiptAsync(test_sender, gas, null, null, sensor_name.ToString(), rate.ToString(), now.ToString()).ConfigureAwait(false); // make transction
                var abiencode = new ABIEncode();
                var test_encode = abiencode.GetABIEncoded(receipt7).ToHex();
                var test_encode2 = abiencode.GetSha3ABIEncodedPacked(receipt7).ToHex(); // encode transction
                if (result == 1)
                {
                    if (listView1.InvokeRequired)
                    {
                        listView1.Invoke((MethodInvoker)delegate ()
                        {
                            listView1.Items.Add("Sensors Setting values");
                            listView1.Items.Add(sensor_name.ToString());
                            listView1.Items.Add(rate.ToString()); // display results

                            listView1.Items.Add(now.ToString());
                        });
                    }

                }
                else
                {
                    listView1.Items.Add("Sensors set values feild");
                }



            }
            watch.Stop(); // stop calculate time
            if (textBox4.InvokeRequired)
            {
                textBox4.Invoke((MethodInvoker)delegate ()
                {
                    textBox4.Text = watch.ElapsedMilliseconds.ToString() + " MS";  // display benchmark
                });
            }
        }

        public async void get_sensors_data()
        {
            var abi = textBox2.Text;
            var address = textBox3.Text;
            var con = web3.Eth.GetContract(abi, address);
            var function = con.GetFunction("get_sensors_data_count");
            var count = await function.CallAsync<int>();
            listView1.Items.Add("----- Get Data -----");
            for (int i = 1; i <= count; i++)
            {
                function = con.GetFunction("get_sensors_data");
                var result = await function.CallAsync<string>(i);
                listView1.Items.Add(result.ToString());
                
            }
        }

        public async void set_data_1_non_zkp()
        {
            // this function simulate sensor reads but with out using zkp verfycation
            // get keys for sensor1
            var watch = new System.Diagnostics.Stopwatch(); // time caluclate
            watch.Start();
            var abi = textBox2.Text; //get abi
            var address = textBox3.Text; // get address
            var con = web3.Eth.GetContract(abi, address); //connect to contract
            Random rand = new Random(); // generate sensor values
            
            string sensor_name = "Hurt_Rate";
            int rate = rand.Next();
            DateTime now = DateTime.Now;
            var function = con.GetFunction("set_reads"); // select function from contract 
            var result = await function.CallAsync<int>(sensor_name,rate.ToString(), now.ToString()); // call function 
            var accounts = await web3.Personal.ListAccounts.SendRequestAsync().ConfigureAwait(false); // select account to make transaction
            var test_sender = "";
            foreach (var x in accounts)
            {
                test_sender = x.ToString();
                break;
            }

            var gas = await function.EstimateGasAsync(sensor_name, rate.ToString(), now.ToString()).ConfigureAwait(false); // calculate gas required to make transaction
            //var trans = await function.SendTransactionAsync(test_sender, "temp", V.ToString(), S.ToString(), N.ToString());
            var receipt7 = await function.SendTransactionAndWaitForReceiptAsync(test_sender, gas, null, null, sensor_name, rate.ToString(), now.ToString()).ConfigureAwait(false); // make transaction
            var abiencode = new ABIEncode();
            var test_encode = abiencode.GetABIEncoded(receipt7).ToHex();
            var test_encode2 = abiencode.GetSha3ABIEncodedPacked(receipt7).ToHex(); // encode tranaction
            if (result == 1)
            {
                if (listView1.InvokeRequired)
                {
                    listView1.Invoke((MethodInvoker)delegate ()
                    {
                        listView1.Items.Add("Sensors Setting values");
                        listView1.Items.Add(sensor_name.ToString());
                        listView1.Items.Add(rate.ToString());
                          // display results
                        listView1.Items.Add(now.ToString());
                    });
                }

            }
            else
            {
                listView1.Items.Add("Sensors set values feild");
            }

            watch.Stop();
            if(textBox4.InvokeRequired)
            {
                textBox4.Invoke((MethodInvoker)delegate ()
                {
                    textBox4.Text = watch.ElapsedMilliseconds.ToString() + " MS"; // benchmark
                });
            }
            

        }

        public async void set_data_2_non_zkp()
        {
            // this function simulate sensor reads but with out using zkp verfycation
            // get keys for sensor1
            var watch = new System.Diagnostics.Stopwatch(); // time caluclate
            watch.Start();
            var abi = textBox2.Text; //get abi
            var address = textBox3.Text; // get address
            var con = web3.Eth.GetContract(abi, address); //connect to contract
            Random rand = new Random(); // generate sensor values

            string sensor_name = "temp";
            int rate = rand.Next();
            DateTime now = DateTime.Now;
            var function = con.GetFunction("set_reads"); // select function from contract 
            var result = await function.CallAsync<int>(sensor_name, rate.ToString(), now.ToString()); // call function 
            var accounts = await web3.Personal.ListAccounts.SendRequestAsync().ConfigureAwait(false); // select account to make transaction
            var test_sender = "";
            foreach (var x in accounts)
            {
                test_sender = x.ToString();
                break;
            }

            var gas = await function.EstimateGasAsync(sensor_name, rate.ToString(), now.ToString()).ConfigureAwait(false); // calculate gas required to make transaction
            //var trans = await function.SendTransactionAsync(test_sender, "temp", V.ToString(), S.ToString(), N.ToString());
            var receipt7 = await function.SendTransactionAndWaitForReceiptAsync(test_sender, gas, null, null, sensor_name, rate.ToString(), now.ToString()).ConfigureAwait(false); // make transaction
            var abiencode = new ABIEncode();
            var test_encode = abiencode.GetABIEncoded(receipt7).ToHex();
            var test_encode2 = abiencode.GetSha3ABIEncodedPacked(receipt7).ToHex(); // encode tranaction
            if (result == 1)
            {
                if (listView1.InvokeRequired)
                {
                    listView1.Invoke((MethodInvoker)delegate ()
                    {
                        listView1.Items.Add("Sensors Setting values");
                        listView1.Items.Add(sensor_name.ToString());
                        listView1.Items.Add(rate.ToString());
                        // display results
                        listView1.Items.Add(now.ToString());
                    });
                }

            }
            else
            {
                listView1.Items.Add("Sensors set values feild");
            }

            watch.Stop();
            if (textBox4.InvokeRequired)
            {
                textBox4.Invoke((MethodInvoker)delegate ()
                {
                    textBox4.Text = watch.ElapsedMilliseconds.ToString() + " MS"; // benchmark
                });
            }


        }

        public int zkp_verfy_no_print(string _V,string _S,String _N)
        {
            // this is zkp vefty function edited to not use io printing 
            var Rnd = new Random();
            var Proof = true;

            var AN = Convert.ToInt64(_N);
            var AS = Convert.ToInt64(_S);
            var BN = Convert.ToInt64(_N);
            var BV = Convert.ToInt64(_V);
            textBoxLog.Clear();
           // textBoxLog.AppendText("=====================" + Environment.NewLine);
            //textBoxLog.AppendText("Check Start" + Environment.NewLine);
            //textBoxLog.AppendText("=====================" + Environment.NewLine);
            

            var NeBudetPovtornoR = new HashSet<int>();
            for (var i = 1; i <= 100; i++)
            {
               // textBoxLog.AppendText("---------------------" + Environment.NewLine);
               // textBoxLog.AppendText("Side A(proves)" + Environment.NewLine);
                //textBoxLog.AppendText("---------------------" + Environment.NewLine);

                var AR = 0;
                do
                {
                    AR = Rnd.Next(1, (int)AN);
                }
                while (NeBudetPovtornoR.Contains(AR));
                NeBudetPovtornoR.Add(AR);
                //textBoxLog.AppendText("Random R = " + Convert.ToString(AR) + Environment.NewLine);

                var AX = ZEROPROOF.FastPowFunc(AR, 2, AN);
                //textBoxLog.AppendText("X = " + Convert.ToString(AX) + Environment.NewLine);

                //textBoxLog.AppendText("Sending to X side B" + Environment.NewLine);

                //---------------------------------------------

                //textBoxLog.AppendText("---------------------" + Environment.NewLine);
                //textBoxLog.AppendText("Side B(verifies)" + Environment.NewLine);
                //textBoxLog.AppendText("---------------------" + Environment.NewLine);

               // textBoxLog.AppendText("Getting X from the side A" + Environment.NewLine);
                var BX = AX;
                //textBoxLog.AppendText("X = " + Convert.ToString(BX) + Environment.NewLine);

                var Bb = Rnd.Next(0, 2);
                //textBoxLog.AppendText("Randomset bit b = " + Convert.ToString(Bb) + Environment.NewLine);

                //textBoxLog.AppendText("Sending to b side A" + Environment.NewLine);

                //---------------------------------------------

                //textBoxLog.AppendText("---------------------" + Environment.NewLine);
                //textBoxLog.AppendText("Side A(proves)" + Environment.NewLine);
                //textBoxLog.AppendText("---------------------" + Environment.NewLine);

                //textBoxLog.AppendText("Getting b from the side B" + Environment.NewLine);
                var Ab = Bb;
                //textBoxLog.AppendText("b = " + Convert.ToString(Ab) + Environment.NewLine);

                long AY = 0;
                if (Ab == 0)
                    textBoxLog.AppendText("Sending to R side B" + Environment.NewLine);
                else
                {
                    AY = AR * AS % AN;
                    //textBoxLog.AppendText("Y = " + Convert.ToString(AY) + Environment.NewLine);
                    //textBoxLog.AppendText("Sending to Y side B" + Environment.NewLine);
                }

                //---------------------------------------------

                //textBoxLog.AppendText("---------------------" + Environment.NewLine);
                //textBoxLog.AppendText("Side B(verifies)" + Environment.NewLine);
                //textBoxLog.AppendText("---------------------" + Environment.NewLine);

                if (Bb == 0)
                {
                    //textBoxLog.AppendText("Getting R from the side A" + Environment.NewLine);
                    var BR = AR;
                    // textBoxLog.AppendText("R = " + Convert.ToString(BR) + Environment.NewLine);
                    // textBoxLog.AppendText("Calculated X = " + Convert.ToString(ZEROPROOF.FastPowFunc(BR, 2, BN)) + Environment.NewLine);
                    // textBoxLog.AppendText("Received X = " + Convert.ToString(BX) + Environment.NewLine);
                    if (BX == ZEROPROOF.FastPowFunc(BR, 2, BN))
                        textBoxLog.AppendText("Party A knows sqrt(X)" + Environment.NewLine);
                    else
                    {
                        // textBoxLog.AppendText("Side A does not know sqrt(X).Party A is not genuine!" + Environment.NewLine);
                        Proof = false;
                        break;
                    }
                }
                else
                {
                    // textBoxLog.AppendText("Getting Y from the side A" + Environment.NewLine);
                    var BY = AY;
                    // textBoxLog.AppendText("Y = " + Convert.ToString(BY) + Environment.NewLine);
                    // textBoxLog.AppendText("Calculated X = " + (BigInteger.Pow(BY, 2) * BV % BN).ToString() + Environment.NewLine);
                    // textBoxLog.AppendText("Received X = " + Convert.ToString(BX) + Environment.NewLine);
                    if (BX == Convert.ToInt64((BigInteger.Pow(BY, 2) * BV % BN).ToString()))
                        textBoxLog.AppendText("Party A knows sqrt(V-1)" + Environment.NewLine);

                    else
                    {
                        //textBoxLog.AppendText("Side A does not know sqrt (V-1). Side A is not genuine!" + Environment.NewLine);
                        Proof = false;
                        break;
                    }
                }

            }
            if (Proof)
            {
                 textBoxLog.AppendText("Party A identified!" + Environment.NewLine);
                return 1;
            }
            else
            {
                 textBoxLog.AppendText("Party A not identified!" + Environment.NewLine);
                return 0;
            }
        }

        public int zkp_verfy(string _V, string _S, String _N)
        {
            var Rnd = new Random();
            var Proof = true;

            var AN = Convert.ToInt64(_N);
            var AS = Convert.ToInt64(_S);
            var BN = Convert.ToInt64(_N);
            var BV = Convert.ToInt64(_V);
            textBoxLog.Clear();
            textBoxLog.AppendText("=====================" + Environment.NewLine);
            textBoxLog.AppendText("Check Start" + Environment.NewLine);
            textBoxLog.AppendText("=====================" + Environment.NewLine);


            var NeBudetPovtornoR = new HashSet<int>();
            for (var i = 1; i <= 100; i++)
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

                var AX = ZEROPROOF.FastPowFunc(AR, 2, AN);
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
                textBoxLog.AppendText("Randomset bit b = " + Convert.ToString(Bb) + Environment.NewLine);

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
            {
                textBoxLog.AppendText("Party A identified!" + Environment.NewLine);
                return 1;
            }
            else
            {
                textBoxLog.AppendText("Party A not identified!" + Environment.NewLine);
                return 0;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            contract();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            set_key2();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            set_key1();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            set_key1_fake();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            set_key2_fake();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
                set_data_1_non_zkp();

            if(radioButton2.Checked == true)
                set_data_1();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
                set_data_2_non_zkp();

            if (radioButton2.Checked == true)
                set_data_2();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            get_sensors_data();
        }

        private void button13_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
