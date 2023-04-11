using Microsoft.EntityFrameworkCore;

namespace UserDataApi.Models {
    public class InitData {
        public static void Initialize(IServiceProvider serviceProvider) {
            using (var context = new UserContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<UserContext>>())) {
                // Look for any data.
                if (context.Users.Any()) {
                    return;   // DB has already been initialized
                }
                context.Users.AddRange(
                    new User {
                        Username = "bigman",
                        Email = "sheisty@gmail.com",
                        RegisterDate = DateTime.Parse("2021-05-15")
                    },
                    new User {
                        Username = "willieshaker",
                        Email = "wsofficial@gmail.com",
                        RegisterDate = DateTime.Parse("2022-06-16")
                    },
                    new User {
                        Username = "botlockUZ",
                        Email = "botlockuz@uz.gov",
                        RegisterDate = DateTime.Parse("2020-10-01")
                    },
                    new User {
                        Username = "trapper",
                        Email = "awesomeyacht@gmail.com",
                        RegisterDate = DateTime.Parse("2023-02-22")
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
