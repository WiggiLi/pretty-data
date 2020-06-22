# pretty-data
GUI for displaying row data from table in MS SQL Server with processing XML data.  
It was created with Windows Forms and .Net Framework 4.7.2

![main form](https://i.ibb.co/TPS30sX/1.png "main form")

![show xml data](https://i.ibb.co/cwjcLpg/2.png "Logo Title Text 1")

Getting information about DB and user for setting connecting string:  
![alt text](https://i.ibb.co/k0d3CR4/image.png "Logo Title Text 1")

For working with XML was used XSLT script running with dynamic library from "Saxon API for .Net" (need install Saxon-HE version 9.9.1.7). 

Data for hiding or translating attributes of XML data is saved in files .xml.

Example 
------

input:  
```  
<uz UZ_id="146862" marka="0" data="2016-04-25" typeuz_id="20" flrazgr="0" ntrn_id="28">  
  <uz1 UZ1_id="2808685" uz_id="146862" uzp_id="0" kp_id="38914" kolvo1="12" kolvo2="2.0000"/>  
</uz>
```

output:  
```
Header uz (id=146862)
UZ_id=146862                   Марка_=0                Дата=2016-04-25                            
typeuz_id=20                   flrazgr=0               ntrn_id=28                                 

Header uz1 (id=2808685)
UZ1_id=2808685                 uz_id=146862            Рег.номер_род._=0                          
Код_прих._=38914               Кол._ЕИ_=12             Кол._упак._=2.0000 
```
