﻿using System;

var encryptor = new RSAEncryptor();
var coinApp = new CoinApp();
var user1 = encryptor.GenerateKeys();
var user2 = encryptor.GenerateKeys();
var user3 = encryptor.GenerateKeys();
Console.WriteLine(ulong.MaxValue);
Console.WriteLine(coinApp.PerformSuperAdminTransaction(user1, user3.Publickey, 0));
Console.WriteLine("Balance: " + coinApp.GetBalanceUser(user1) + "  |  " + "Balance: " + coinApp.GetBalanceUser(user3));

//for (int i = 0; i < 10; i++)
//{
//    coinApp.PerformTransaction(user1, user2.Publickey, i);
//    Console.WriteLine("Balance: "+ coinApp.GetBalanceUser(user1) + "  |  " + "Balance: " + coinApp.GetBalanceUser(user2));
//}

//foreach(var a in coinApp.GetTransactions(user2))
//{
//    Console.WriteLine(a.Data.Data);
//}



Console.WriteLine("Good!");


