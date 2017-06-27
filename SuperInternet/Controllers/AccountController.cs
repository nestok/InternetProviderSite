using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data.Entity;
using System.Security.Cryptography;
using SuperInternet.Models;

namespace SuperInternet.Controllers
{
    public class AccountController : Controller
    {
        ServicesContext db = new ServicesContext();
        private ActionResult ErrorView(string errorMessage)
        {
            ViewBag.ErrorMessage = errorMessage;
            return View("../Shared/Error");
        }
        public ActionResult Authorization()
        {
            if (Session["User"] != null)
            {
                ViewBag.SelectedUser = Session["User"];
                return View("PersonalArea");
            }
            Session["User"] = null;
            return View();
        }

        [HttpPost]
        public ActionResult Authorization(UnknownUser unUser)
        {
            User user = db.Users.FirstOrDefault(u => (u.Email == unUser.Email));
            if (user == null)
                return ErrorView("Пользователь с такой почтой не найден");

            byte[] inputPas = Encoding.Unicode.GetBytes(unUser.Password);
            SHA1 sha = new SHA1CryptoServiceProvider();
            inputPas = sha.ComputeHash(inputPas);
            if (!inputPas.SequenceEqual(user.Password))
                return ErrorView("Вы ввели неверный пароль");

            Session["User"] = user;
            ViewBag.User = user;
            ViewBag.SelectedUser = user;

            return View("PersonalArea");
        }

        [HttpGet]
        public ActionResult PersonalArea(int? id)
        {
            ViewBag.User = Session["User"];
            if (id != null)
            {
                User user = db.Users.Find(id);
                if (user == null)
                    return HttpNotFound();
                ViewBag.SelectedUser = user;
            }
            else if (Session["User"] != null)
                ViewBag.SelectedUser = Session["User"];
            else
                return HttpNotFound();

            return View();
        }

        public ActionResult ExitPersonalArea()
        {
            Session["User"] = null;
            return View("Authorization");
        }

        public ActionResult PersonalAreaEdit()
        {
            ViewBag.User = Session["User"];
            return View();
        }

        [HttpGet]
        public ActionResult DeleteUser()
        {
            User user = (User)Session["User"];
            if ((user == null) || (user.Role != UserRole.ADMIN))
                return HttpNotFound();
            IEnumerable<User> users = db.Users;
            ViewBag.Users = users;
            return View(db.Users);
        }

        [HttpPost]
        public ActionResult DeleteUser(int? id)
        {
            if (id == null)
                return HttpNotFound();

            User u = db.Users.Find(id);
            if (u != null)
            {
                db.Users.Remove(u);
                db.SaveChanges();
            }
            return Redirect("~/Account/DeleteUser");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(PasswordChangeEntity pasEntity)
        {
            User user = (User)Session["User"];

            byte[] pas = Encoding.Unicode.GetBytes(pasEntity.OldPassword);
            SHA1 sha = new SHA1CryptoServiceProvider();
            pas = sha.ComputeHash(pas);
            if (!pas.SequenceEqual(user.Password))
                return ErrorView("Невено введён пароль");

            if (String.Compare(pasEntity.NewPassword, pasEntity.RepeatNewPassword, false) != 0)
                return ErrorView("Вы неправильно повторили пароль");

            pas = Encoding.Unicode.GetBytes(pasEntity.NewPassword);
            user.Password = sha.ComputeHash(pas);

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            Session["User"] = user;
            ViewBag.SelectedUser = user;
            ViewBag.User = user;
            return View("PersonalArea");
        }

        [HttpPost]
        public ActionResult PersonalAreaEdit(User newUserInfo)
        {
            User user = (User)Session["User"];
            user.Nickname = newUserInfo.Nickname;
            user.Sername = newUserInfo.Sername;
            user.Name = newUserInfo.Name;
            user.Patronymic = newUserInfo.Patronymic;
            user.Tarif = newUserInfo.Tarif;
            user.Telephone = newUserInfo.Telephone;
            user.Address = newUserInfo.Address;

            HttpPostedFileBase avatar = Request.Files["Image"];
            user.Image = avatar.FileName;
            SaveStreamToFile("E:\\ОАИП!!!\\ООП\\SuperInternet\\SuperInternet\\Images\\" + avatar.FileName, avatar.InputStream);

            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            Session["User"] = user;
            ViewBag.SelectedUser = user;
            ViewBag.User = user;
            return View("PersonalArea");
        }

        private void SaveStreamToFile(string filename, Stream stream)
        {
            if (stream.Length != 0)
                using (FileStream fileStream = System.IO.File.Create(filename, (int)stream.Length))
                {
                    // Размещает массив общим размером равным размеру потока
                    // Могут быть трудности с выделением памяти для больших объемов
                    byte[] data = new byte[stream.Length];

                    stream.Read(data, 0, (int)data.Length);
                    fileStream.Write(data, 0, data.Length);
                }
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(UnknownUser unUser)
        {
            if (db.Users.FirstOrDefault(u => (u.Nickname == unUser.Nickname)) != null)
                return ErrorView("Пользователь с таким логином уже существует");

            if (db.Users.FirstOrDefault(u => (u.Email == unUser.Email)) != null)
                return ErrorView("Пользователь с такой почтой уже существует");

            if (String.Compare(unUser.Password, unUser.RepeatPassword, false) != 0)
                return ErrorView("Неправильно введён пароль");

            unUser.Role = UserRole.CLIENT;
            User currentUser = unUser.ToUser();
            db.Users.Add(currentUser);
            db.SaveChanges();

            Session["User"] = currentUser;
            ViewBag.SelectedUser = currentUser;
            ViewBag.User = currentUser;
            return View("PersonalArea");
        }
	}
}