using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Zamger2._0.Data
{
    public static class Seed
    {
        #region snippet_Initialize
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();

            var profesor = await EnsureUser(serviceProvider, testUserPw, "profesor@etf.unsa.ba");
            var student = await EnsureUser(serviceProvider, testUserPw, "student@etf.unsa.ba");

            await EnsureRole(serviceProvider, profesor, "profesor");
            await EnsureRole(serviceProvider, student, "student");

            await AddSubject(serviceProvider, "Tehnologije sigurnosti", profesor);
            await AddSubject(serviceProvider, "Računarski sistemi u realnom vremenu", profesor);
            await AddSubject(serviceProvider, "Metode i primjena vještačke inteligencije", profesor);
            await AddSubject(serviceProvider, "Napredni softver inženjering", profesor);

            //Seed rest of the data
            SeedDB(context);

        }

       

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }
        private static async Task<string> AddSubject(IServiceProvider serviceProvider, string subject, string profesor)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();

            var s = context.Subjects.FirstOrDefault(m => m.Name.Equals(subject) && m.ProfesorId.Equals(profesor));
           
            if (s == null)
            {
                //System.Diagnostics.Debug.WriteLine("hello");
                //System.Diagnostics.Debug.WriteLine(exam.Name);
                context.Subjects.Add(new Subject()
                {
                    Name = subject,
                    ProfesorId = profesor
                });

                //context.SaveChanges();

            }


            return subject;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
        #endregion

        public static void SeedDB(ApplicationDbContext context)
        {
     
            context.SaveChanges();
        }
    }
}
