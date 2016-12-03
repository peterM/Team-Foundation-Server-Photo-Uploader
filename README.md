# Team-Foundation-Server-Photo-Uploader
Provide ability to batch upload photos from many sources to Team Foundation Server

Sometime Company what uses TFS want to migrate user profile photos from Active Directory or another sources into TFS.
This is now possible only when User update profile picture by self.

In repo are two providers out-of-box:
*  ```ActiveDirectoryPhotoProvider  ```
*  ```FileSystemPhotoProvider  ```

Or you can create your own by implementing interface ```IPhotoProvider ```

Currently behvior is to skip TFS identities with already uploaded photo to prevent photo overriding.
