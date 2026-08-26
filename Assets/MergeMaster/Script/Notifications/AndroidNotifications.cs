// using System.Collections;
// using System.Collections.Generic;

// #if UNITY_ANDROID || UNITY_EDITOR
// // using Unity.Notifications.Android;
// #endif
// using UnityEngine;

// public class AndroidNotifications : MonoBehaviour
// {
//     public static AndroidNotifications Instance;
//     private void Awake()
//     {
//         Instance = this;
//     }

// #if UNITY_ANDROID || UNITY_EDITOR
//     // AndroidNotification notificationReminder;
//     // AndroidNotification notificationDailyRewards;
//     public int TimeToRemindPlayGame;
//     void Start()
//     {

//         if (!PlayerPrefs.HasKey("DayTimeDailyRewardsSent") && !PlayerPrefs.HasKey("DayTimeReminderSent"))
//         {
//             PlayerPrefs.SetString("DayTimeDailyRewardsSent", System.DateTime.Now.ToString());
//             PlayerPrefs.SetString("DayTimeReminderSent", System.DateTime.Now.ToString());
//         }
//         SendReminderNotification();
//     }

//     [Sirenix.OdinInspector.Button]
//     public void SendDailyRewardNotification()
//     {
//         if (System.DateTime.Parse(PlayerPrefs.GetString("DayTimeDailyRewardsSent")) <= System.DateTime.Now)
//         {
//             if (MMSDailyRewardsTime.Instance.timeDelay == 0) return;
//             SetUpNotifications(notificationDailyRewards);
//             SetIcon(notificationDailyRewards);
//             MMSDailyRewardsTime.Instance.ActiveCaculateTimeToHourMinus();
//             SendNotifications(notificationDailyRewards, "Daily Reward !", "The time is up. Play the game to get new reward !", (int)MMSDailyRewardsTime.Instance.timeDelay);
//             Debug.Log("SendDailyRewardNotification");
//         }
//     }

//     [Sirenix.OdinInspector.Button]
//     void SendReminderNotification()
//     {
//         if (System.DateTime.Parse(PlayerPrefs.GetString("DayTimeReminderSent")) <= System.DateTime.Now)
//         {
//             SetUpNotifications(notificationReminder);
//             SetIcon(notificationReminder);
//             SendNotifications(notificationReminder, "Do you miss some battles ?", "Please come back to be a hero !", TimeToRemindPlayGame);
//             Debug.Log("Notification reminder sent");
//         }
//     }

//     void SetUpNotifications(AndroidNotification notification)
//     {
//         var channel = new AndroidNotificationChannel()
//         {
//             Id = "channel_id",
//             Name = "Notification Channel",
//             Importance = Importance.Default,
//             Description = "Reminder notifications",
//         };
//         AndroidNotificationCenter.RegisterNotificationChannel(channel);
//         notification = new AndroidNotification();
//     }
//     void SendNotifications(AndroidNotification notification, string title, string text, int _timeToRemind)
//     {
//         notification.Title = title;
//         notification.IntentData = "";
//         notification.Text = text;
//         notification.FireTime = System.DateTime.Now.AddSeconds(_timeToRemind);
//         AndroidNotificationCenter.SendNotification(notification, "channel_id");

//         // check để ko popup noti lại nếu chưa đủ tgian
//         if (notification.Title == "Do you miss some battles ?")
//         {
//             PlayerPrefs.SetString("DayTimeReminderSent", notification.FireTime.ToString());
//         }
//         else if (notification.Title == "Daily Reward !")
//         {
//             PlayerPrefs.SetString("DayTimeDailyRewardsSent", notification.FireTime.ToString());
//         }
//         // sent notification
//     }
//     // void SetIcon(AndroidNotification notification, string smallIcon = "icon_small", string largeIcon = "icon_large")
//     // {
//     //     notification.SmallIcon = smallIcon;
//     //     notification.LargeIcon = largeIcon;
//     // }
//     void NotificationReceivedCallback()
//     {
//         AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler =
//             delegate (AndroidNotificationIntentData data)
//             {
//                 var msg = "Notification received : " + data.Id + "\n";
//                 msg += "\n Notification received: ";
//                 msg += "\n .Title: " + data.Notification.Title;
//                 msg += "\n .Body: " + data.Notification.Text;
//                 msg += "\n .Channel: " + data.Channel;
//                 Debug.Log(msg);
//             };

//         AndroidNotificationCenter.OnNotificationReceived += receivedNotificationHandler;
//     }
// #endif
// }
