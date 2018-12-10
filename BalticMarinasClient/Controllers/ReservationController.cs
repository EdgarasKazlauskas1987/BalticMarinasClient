﻿using BalticMarinasClient.ApiClient;
using BalticMarinasClient.Models;
using BalticMarinasClient.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BalticMarinasClient.Controllers
{
    public class ReservationController : Controller
    {
        BookMarinaClient bookmarinaClient = new BookMarinaClient();
        EmailClient emailClient = new EmailClient();

        [Authorize(Roles = "User")]
        public IActionResult Index()
        {
            int customerId = Int32.Parse(User.FindFirst("UserId").Value);

            var items = bookmarinaClient.GetAllReservationsByCustomerId(customerId).Result;
            ViewBag.ItemsList = items;
            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult Reserve(int berthId, string checkIn, string checkOut, string email)
        {
            int customerId = Int32.Parse(User.FindFirst("UserId").Value);

            Reservation reservation = new Reservation() { BerthId = berthId, CustomerId = customerId, CheckIn = checkIn, CheckOut = checkOut };
            bookmarinaClient.CreateReservation(reservation);
            emailClient.SendConfirmationEmail(Constants.ConfirmedEmailBody, email);
            return RedirectToAction("Confirmation", "Reservation");
        }

        public IActionResult PersonalInformation(int berthId, string checkIn, string checkOut)
        {
            bool isLoggedIn = HttpContext.User.Identity.IsAuthenticated;

            if(HttpContext.User.Identity.IsAuthenticated == true)
            {
                string email = User.FindFirst("Email").Value;
                ViewBag.BerthId = berthId;
                ViewBag.CheckIn = checkIn;
                ViewBag.CheckOut = checkOut;
                ViewBag.Email = email;
                return View();
            }
            else
            {
                return RedirectToAction("LoginOrRegister", "User");
            }
        }

        [Authorize(Roles = "User")]
        public IActionResult Payment(int berthId, string checkIn, string checkOut, string email)
        {
            ViewBag.BerthId = berthId;
            ViewBag.CheckIn = checkIn;
            ViewBag.CheckOut = checkOut;
            ViewBag.Email = email;
            return View();
        }
    }
}