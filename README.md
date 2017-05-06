# Celestial Altium Database Desktop Manager
Windows software that enables you to register for access to the Live Azure database, keep your firewall rule up to date, and create the connection data file for Altium to use.


# Very Easy to Use
1. Download and install: http://altiumservices.azurewebsites.net/Desktop/download.html

2. Register an Account:

![image](https://cloud.githubusercontent.com/assets/1425724/25770493/1be0c16a-31f4-11e7-9032-3cdaa514beed.png)

Just enter your details, and an authorisation code will be Emailed to you automatically to make sure you're a real person!

3. Activate your account, using the code emailed to you:

![image](https://cloud.githubusercontent.com/assets/1425724/25770514/a557f68e-31f4-11e7-8ac6-78db17f99eac.png)

Activating your account creates an Azure Database Firewall entry for you, allowing you to connect to the database from Altium.

4. Browse to the location you cloned/downloaded the Celestial Altium Library, and click the update button.

![image](https://cloud.githubusercontent.com/assets/1425724/25770496/2dbde48a-31f4-11e7-91f1-98d371d4907a.png)

This will setup the altium database file to use your new credentials to access Azure.

# Dynamic IP Address Users
If your IP address changes, all you need to do is click the "Azure Access Blocked" icon in the top right of the window.

![image](https://cloud.githubusercontent.com/assets/1425724/25770555/9a7f2f24-31f5-11e7-8874-f92809036f99.png)

The software checks your firewall rule every hour, and every time the software starts. Click the icon and your firewall rule will be updated, and then re-checked.

![image](https://cloud.githubusercontent.com/assets/1425724/25770571/d5323f44-31f5-11e7-928c-c001f508b511.png)


# The Plan
This software has been a long time coming, I've been trying to manually had 10-20 people per day to Azure, and thats pretty time consuming. So this is a big step forwards, if everything goes smoothly, especially with the very limited amounts of free time I've had recently.

Here are some high priority features I'd like to add to the software, if this is something you feel you can contribute to, please let me know:
* Use of GitSharp to keep the local altium files up to date automatically
* Creation of local database for altium to talk to (webservices for view data are mostly in place already)
* Syncing of components from Azure database to local database
* Tool to add components to local database from supplier pages (digikey/mouser/element14)
* Tool to add views to local database
* Tools to create a pull request of sorts for locally added components (web services mostly in place)

This should make this software really powerful and enhance the whole altium library capabilities, making it much easier for users to contribute.
