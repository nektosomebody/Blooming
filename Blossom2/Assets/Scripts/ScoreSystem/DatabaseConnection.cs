using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;

namespace Assets.Scripts.ScoreSystem
{
    public class DatabaseConnection
    {
        DatabaseReference root;
        public DatabaseConnection() {
            root = FirebaseDatabase.DefaultInstance.RootReference;
        }

        public void AddNewScore(string userId, int score)
        {
            root.Child("leaderboard").Child(userId).SetValueAsync(score);
        }
    }
}
