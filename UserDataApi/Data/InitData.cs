using Microsoft.EntityFrameworkCore;
using UserDataApi.Models;

namespace UserDataApi.Data
{
    public class InitData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new UserContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<UserContext>>()))
            {
                // Look for any data.
                if (context.Users.Any())
                {
                    return;   // DB has already been initialized
                }
                context.Users.AddRange(
                    new User
                    {
                        Username = "bigman",
                        Email = "sheisty@gmail.com",
                        RegisterDate = DateTime.Parse("2021-05-15")
                    },
                    new User
                    {
                        Username = "willieshaker",
                        Email = "wsofficial@gmail.com",
                        RegisterDate = DateTime.Parse("2022-06-16"),
                        DisplayName = "Willie Shaker"
                    },
                    new User
                    {
                        Username = "botlockUZ",
                        Email = "botlockuz@uz.gov",
                        RegisterDate = DateTime.Parse("2020-10-01")
                    },
                    new User
                    {
                        Username = "trapper",
                        Email = "awesomeyacht@gmail.com",
                        RegisterDate = DateTime.Parse("2023-02-22"),
                        DisplayName = "Awesome_Trapper"
                    }
                );
                context.Entries.AddRange(
                    new Entry {
                        UserId = 1,
                        Id = 1,
                        EntryText = "My first entry!"
                    },
                    new Entry {
                        UserId = 3,
                        Id = 1,
                        EntryText = "Out to get some groceries."
                    },
                    new Entry {
                        UserId = 1,
                        Id = 2,
                        EntryText = "My second entry! I love this website!"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
