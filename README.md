# Discord.Zendesk
A mindbogglingly simple integration of Discord and Zendesk (using Webhooks)

## Compiling
### Using Visual Studio 2017
* [Visual Studio 2017](https://www.visualstudio.com/vs/)
* [.NET Core SDK](https://www.microsoft.com/net/learn/get-started/windows)

> This build was compiled on Windows 10 x64 using Visual Studio 2017. It was deployed and tested on a Ubuntu 18.04 LTS x64 virtual machine running .NET Core, nginx, and Let's Encrypt (using certbot).

## Deploying
### Creating a Web Server
1. Make sure to [follow this guide](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?tabs=aspnetcore2x&view=aspnetcore-2.1) on how to properly configure and host the ASP.NET Core application. It is also recommended that you configure Let's Encrypt SSL within nginx.
2. Make note of the URL of your webserver as we will need it later.

### Create a Webhook
1. In your selected server, open up `Server Settings`.
2. Click on `Webhooks` and select `Create Webhook`.
3. `Edit` your Webhook with your desired settings.

![Discord Webhook Edit Settings](http://screenshots.initialservers.com/jrainier/0c6cc448223d4db39ba96553c098cc619605.png)

4. Copy the `Webhook URL` and update your `WebHookUrl` in `appsettings.json`

### Setting up Zendesk
1. Login as an administrator and head over to the `Extensions` section(`/agent/admin/extensions`) and click on `add target`.
2. Select `HTTP Target`, give it a title, set the `Url` to your Web Server's hostname. Append `/api/ticket` e.g. if `https://localhost` was your web server's hostname, it would look like: `https://localhost/api/ticket`
3. Set the `Method` to `POST` and the `Content type` to `JSON`
4. Select `Create target` and hit `Submit`
5. Navigate under `BUSINESS RULES` and find `Triggers`.
6. Click on `Add trigger`
7. Give your trigger a name and a description.
8. Under the section `Meet ANY of the following conditions` set the following options ![Zendesk Trigger Conditions](http://screenshots.initialservers.com/jrainier/4c05f66e8b729ea61baa72fcbcc6c23dd5ed.png)
9. Under `Actions` select `Notify target` and then the name of your new trigger
10. Set the JSON body to the following (ignore Zendesk if it notifies you of any string/integer errors):
```javascript
{
	"status": "{{ticket.status}}",
	"id": {{ticket.id}},
	"updated": "{{ticket.updated_at_with_timestamp}}",
	"title": "{{ticket.title}}",
	"requester": "{{ticket.requester.name}}",
	"comment": "{{ticket.latest_comment}}",
	"sender": "{{current_user.name}}",
	"url": "{{ticket.link}}"
}
```
11. Click `Save`. Update or open a new ticket and check your support channel on Discord!
