# How To Get OpenApi And Use It

> This article is translated by Bing and, if there is a problem, welcome feedback via the right link:<https://github.com/AZEXIO/APIDocument/issues>

This article will describe the work and knowledge that developers need to prepare in advance to invoke APIs that contain user private information.

## Apply for OpenApi permission

1. Login Azex

  Visit <https://www.azex.io> and login account, enter "account set->API Management-> Add Api" to create your OpenApi.

2. Get Apikey and Secret

  After successfully passing two certification, you can obtain Apikey and secret.

  **Note: Apikey and secret are only shown once, please keep them properly.**

  **Note: Apikey and secret are important data related to the user's assets, please keep them properly and beware of leaks.**

## How to use Apikey and secret

After the procedure described in the previous section, the developer has obtained the two parameters Apikey and Secret necessary to invoke OpenApi.

This section describes how to use these two values to invoke Openapi.

**Again: Apikey and secret are important data about the user's assets, please keep them properly and beware of leaks.**

### HTTP API

There is a class of APIs in the HTTP API that are related to the user's personal assets, and calls to this API require Apikey and secret to sign the requested data.

#### How to use

##### ApiKey

Apikey is passed directly in the header of the request when the request is made. Suppose the Apikey is 27783.xxxxxxxxxxx.The header is

Name          | Value
------------- | -------------------------
Authorization | OPENAPI 27783.xxxxxxxxxxx

Note that there is a space between OPENAPI and Apikey.

##### Secret

The secret is used to encrypt the requested parameters at request, to ensure that the Azex received by the user is indeed emitted and not tampered with.

The detailed signature process is as follows.

1).Suppose the requested parameter name value pair is the following

Name | Value
---- | ---------------
b    | azex,is,perfect
a    | 1
as   | 3
ae   | 2
z    | 3.1415926

2).Gets the current client's Unix timestamp number of seconds, appended to the request parameter, the request parameter name value pair becomes the following

Name      | Value
--------- | ---------------
b         | azex,is,perfect
a         | 1
as        | 3
ae        | 2
z         | 3.1415926
timestamp | 1531137017

3).The above name values are sorted in ascending order by name, and the results are as follows, and note that the following steps are compared with step 2nd:

Name      | Value
--------- | ---------------
a         | 1
ae        | 2
as        | 3
b         | azex,is,perfect
timestamp | 1531137017
z         | 3.1415926

4).the name value pairs that are generated in the previous step are connected by using `&` and `=`, and the attached text is as follows:

```bash
a=1&ae=2&as=3&b=azex,is,perfect&timestamp=1531137017&z=3.1415926
```

5).Using secret and the string generated by step 4th as plaintext to compute the sign with `HmacSHA256`.

Assuming secret is `17184178f3334842a75c15c1d1d4e666`, the sign will be:

```bash
b72ba29328442e669851414cc0d894156dcee8c324b272b5819cc149ef877e58
```

6).The sign value that is generated in the previous step and all the data in step 3rd are submitted as Form post, and the call can be completed.

7).This completes the authentication process necessary to subscribe the user's personal transaction information, and also:

Developers can use the sign value in this sample to match their computed sign values to detect if they are doing the right way, and to compute the sign value through the online tool on the right-hand side: <https://www.freeformatter.com/hmac-generator.html>

Developers can use search engine or the right link to find the method to get the UNIX timestamp second number in sone development language with the right link : <https://www.epochconverter.com/>

### WebSocket

It is necessary to use the Apikey and secret if you need to subscribe to the personal transaction information from WebSocket.

The detailed steps are as follows:

1). Suppose the user's apikey is `81.67AAA2F6041D408D9868387A8904431D`, Secret is `2288987EFDB54F848D7BACCE1288FC9A`.

2). Use secret and Apikey concat with `Authorization=` as `Authorization=81.67AAA2F6041D408D9868387A8904431D` to plaintext to compute the sign with `HmacSHA256`.

The sign value can be obtained.

```bash
057c4c6770d565aa236f87706053bd51512862443062e471bd3243a60ed8eef2
```

3).passing ApiKey and sign as parameters on the url, the resulting WebSocket link is

```bash
wss://ws.azex.io?Authorization=81.67AAA2F6041D408D9868387A8904431D&sign=057c4c6770d565aa236f87706053bd51512862443062e471bd3243a60ed8eef2
```

4).This completes the authentication process required to subscribe to the user's personal transaction information.
