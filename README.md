# Notifier
The application is a desktop version of the task notifier. Connects to the shared s3 storage, where all tasks are stored in json files.

## How to connect to the storage
First of all, you need to set up a storage connection. 
The configuration file is located in the root directory ```ConnectionConfig.xml```. There you need to specify the storage connection settings.
### Example of ```ConnectionConfig.xml```
 ```
<S3Connection>
	<BucketName>bucket_example</BucketName>
	<ServiceURL>https://s3.yandexcloud.net</ServiceURL>
	<AuthenticationRegion>us-east-1</AuthenticationRegion>
	<AccessKey>YChaP1111AAAEXAMPLEACCESSKEY</AccessKey>
	<SecretKey>YCjAfA22222AAAEXAMPLESECRETKEY</SecretKey>
</S3Connection>
```
```BucketName``` - The name of your existing bucket in object storage
```ServiceURL``` - Host for connecting to object storage
```AuthenticationRegion``` - Authentication region of your host
```AccessKey``` - Access key from a service account with access to object storage
```SecretKey``` - Secret key from a service account with access to object storage
