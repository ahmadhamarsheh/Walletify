﻿using Microsoft.AspNetCore.Mvc;

namespace Walletify.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult CreateTransaction()
        {
            return View();
        }

        public IActionResult UpdateTransaction()
        {
            return View();
        }

        [HttpPut]
        public IActionResult UpdaetTransaction()
        {
            return View();
        }

        [HttpDelete]
        public IActionResult DeleteTransaction()
        {
            return View();
        }

        public IActionResult GetAllTransactions()
        {
            return View();
        }

        public IActionResult GetTransaction(int id)
        {
            return View();
        }
    }
}
