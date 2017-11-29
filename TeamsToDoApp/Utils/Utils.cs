using Bogus;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TeamsSampleTaskApp.DataModel;
using TeamsToDoApp.Utils;

namespace TeamsSampleTaskApp.Utils
{
    public static class Utils
    {

        public static TaskItem CreateTaskItem()
        {
            var faker = new Faker();
            return new TaskItem()
            {
                Title = faker.Commerce.ProductName(),
                Description = faker.Lorem.Sentence(),
                Assigned = $"{faker.Name.FirstName()} {faker.Name.LastName()}",
                Guid = Guid.NewGuid().ToString()
            };
        }

        public static async Task CallWebhook(string webhook, TaskItem item)
        {
            string json = TeamsSampleTaskApp.Utils.Utils.GetJsonActionCard(item);

            //prepare the http POST
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            using (var response = await client.PostAsync(webhook, content))
            {
                // Check response.IsSuccessStatusCode and take appropriate action if needed.
            }
        }

        public static string GetJsonActionCard(TaskItem task)
        {
            //prepare the json payload
            return @"
                {
                    'summary': 'A task is added.',
                    'sections': [
                        {
                            'activityTitle': 'New Task Craeted!',
                            'facts': [
                                {
                                    'name': 'Title:',
                                    'value': '" + task.Title + @"'
                                },
                                {
                                    'name': 'Description:',
                                    'value': '" + task.Description + @"'
                                },
                                {
                                    'name': 'Assigned To:',
                                    'value': '" + task.Assigned + @"'
                                }
                            ]
                        }
                    ],
                    'potentialAction': [
                        {
                            '@context': 'http://schema.org',
                            '@type': 'ViewAction',
                            'name': 'View Task Details',
                            'target': [
                                '" + AppSettings.BaseUrl + "/task/detail/" + task.Guid + @"'
                            ]
                        },
                        {
                          '@type': 'ActionCard',
                          'name': 'Update Title',
                          'inputs': [
                            {
                              '@type': 'TextInput',
                              'id': 'title',
                              'isMultiline': true,
                              'title': 'Please enter new title'
                            }
                          ],
                          'actions': [
                            {
                              '@type': 'HttpPOST',
                              'name': 'Update Title',
                              'isPrimary': true,
                              'target': '" + AppSettings.BaseUrl + "/task/update?id=" + task.Guid + @"',
                              'body': '{""Title"":""{{title.value}}""}',
                                'bodyContentType': 'application/json'
                            }
                          ]
                        }
                    ]}";
        }
    }

    public class TabContext
    {
        public string ChannelId { get; set; }
        public string CanvasUrl { get; set; }
    }

}