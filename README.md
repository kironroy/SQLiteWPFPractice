# SQLiteWPFPractice
A small practice project on WPF and SQLite

## Would Framework: 
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

| Client | 
| --- | --- |
| &#128273; Id |
| HoursRate |
| Email |
| PreBill |
| HasCutOff |
| CutOff   |
| MinimumFours  |  
| BillingIncrement   |
| RoundUpAfterXMinutes    |



[//]: # (Defaults)


| Defaults | 
| --- | --- 
| &#128273; Id 
| HourlyRatel 
| PreBill  
| CutOff   
| MinimumHours    
| BillingIncrement   
| RoundUpAfterXMinutes  



[//]: # (Payment)


| Payment | 
| --- | --- 
| &#128273; Id 
| ClientId
| Hours  
| Amount   
| Date   


[//]: # (Work)


| Work | 
| --- | --- 
| &#128273; Id 
| ClientId
| Hours  
| Title  
| Description
| DateEntered
| Paid
| PaymentId   
 




 