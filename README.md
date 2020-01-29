# SQLiteWPFPractice
A small practice project on WPF and SQLite

## W: 
Small application that will be used to track hours
spent working for a client.
The billables related with the hours and the payements
made for those hours.

---

## O:
 * _WPF_ Front end (No MVVM)
 * _SQLite_ Backend
 * Small as possible
 * App will not be fullscreen
 
## U: 
User does most of the work on the main page

## L:
 * Minimize to System Tray
 * Deploy SQLite with application
 * WPF Parent/child forms
 * WPF Menu bars  

## D:

#### SQLite tables:


[//]: # (Client Table)

| Client | data type |  
| --- | --- |
| &#128273; Id | INTEGER |
| HourlyRate | REAL |
| Email | TEXT |
| PreBill | INTEGER |
| HasCutOff | INTEGER |
| CutOff   | INTEGER |
| MinimumFours  | REAL |  
| BillingIncrement   | REAL |
| RoundUpAfterXMinutes    | INTEGER|



[//]: # (Defaults)


| Defaults | data type | 
| --- | --- |
| &#128273; Id | INTEGER |
| HourlyRatel | REAL |
| PreBill  | INTEGER |
| CutOff  | INTEGER | 
| MinimumHours | REAL |   
| BillingIncrement  | REAL | 
| RoundUpAfterXMinutes  | INTEGER|



[//]: # (Payment)


| Payment | data type |  
| --- | --- 
| &#128273; Id | INTEGER |
| ClientId | INTEGER|
| Hours  | REAL |
| Amount   | REAL |
| Date   | REAL |


[//]: # (Work)


| Work | data type | 
| --- | --- 
| &#128273; Id | INTEGER |
| ClientId | INTEGER |
| Hours  | REAL |
| Title  | TEXT |
| Description | TEXT |
| DateEntered | REAL |
| Paid | INTEGER|
| PaymentId | INTEGER |    







 