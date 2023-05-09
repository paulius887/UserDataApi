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
                        RegisterDate = DateTime.Parse("2021-05-15"),
                    },
                    new User
                    {
                        Username = "willieshaker",
                        Email = "wsofficial@gmail.com",
                        RegisterDate = DateTime.Parse("2022-06-16"),
                        DisplayName = "Willie Shaker",
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
                context.Comments.AddRange(
                    new Comment {
                        UserId = 1,
                        Id = 1,
                        BookId = 1,
                        CommentText = "Hey guys, I just found this Facebook tool that gives you free likes! {LINK REMOVED}"
                    },
                    new Comment {
                        UserId = 3,
                        Id = 1,
                        BookId = 1,
                        CommentText = "Not to cherry pick (I do love picking cherries), but this book doesn't have the best landscape descriptions."
                    },
                    new Comment {
                        UserId = 1,
                        Id = 2,
                        BookId = 2,
                        CommentText = "I hate this bloody book!"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
