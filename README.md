# ProductsAPI
Clone the repository
```bash
git clone --recurse-submodules https://github.com/GediminasKripas/ProductTrackerSoap.git
```
Change directory:
```bash
cd ProductTrackerSoap
```

Launch docker containers:
```bash
docker-compose up -d
```
## Swagger
Url to swagger:
```bash
http://localhost:80/swagger/index.html
```
## Soap Requests
GetProduct
```bash
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:GetProduct>
         <tem:id>?</tem:id>
      </tem:GetProduct>
   </soapenv:Body>
</soapenv:Envelope>
```
GetProductSuplier
```bash
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:GetProductSuplier>
         <tem:id>?</tem:id>
      </tem:GetProductSuplier>
   </soapenv:Body>
</soapenv:Envelope>
```
GetProducts
```bash
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:GetProducts/>
   </soapenv:Body>
</soapenv:Envelope>
```
PostProduct
```bash
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/" xmlns:prod="http://www.example.com/product">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:PostProduct>
         <tem:product>
            <!--Optional:-->
            <prod:id>?</prod:id>
            <prod:itemName>?</prod:itemName>
            <!--Optional:-->
            <prod:kCal>?</prod:kCal>
            <prod:price>?</prod:price>
            <!--Optional:-->
            <prod:supplierId>?</prod:supplierId>
            <!--Optional:-->
            <prod:url>?</prod:url>
         </tem:product>
      </tem:PostProduct>
   </soapenv:Body>
</soapenv:Envelope>
```
PostProductContacts
```bash
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/" xmlns:prod="http://www.example.com/product">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:PostProductContacts>
         <tem:body>
            <!--Optional:-->
            <prod:id>?</prod:id>
            <prod:itemName>?</prod:itemName>
            <!--Optional:-->
            <prod:kCal>?</prod:kCal>
            <prod:price>?</prod:price>
            <!--Optional:-->
            <prod:supplier>
               <prod:email>?</prod:email>
               <prod:id>?</prod:id>
               <prod:name>?</prod:name>
               <prod:number>?</prod:number>
               <prod:surname>?</prod:surname>
            </prod:supplier>
            <!--Optional:-->
            <prod:supplierId>?</prod:supplierId>
            <!--Optional:-->
            <prod:url>?</prod:url>
         </tem:body>
      </tem:PostProductContacts>
   </soapenv:Body>
</soapenv:Envelope>
```
DeleteProduct
```bash
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:DeleteProduct>
         <tem:id>?</tem:id>
      </tem:DeleteProduct>
   </soapenv:Body>
</soapenv:Envelope>
```
DeleteProductContacts
```bash
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:DeleteProductContacts>
         <tem:id>?</tem:id>
      </tem:DeleteProductContacts>
   </soapenv:Body>
</soapenv:Envelope>
```
PutProduct
```bash
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/" xmlns:prod="http://www.example.com/product">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:PutProduct>
         <tem:id>?</tem:id>
         <tem:product>
            <!--Optional:-->
            <prod:id>?</prod:id>
            <prod:itemName>?</prod:itemName>
            <!--Optional:-->
            <prod:kCal>?</prod:kCal>
            <prod:price>?</prod:price>
            <!--Optional:-->
            <prod:supplierId>?</prod:supplierId>
            <!--Optional:-->
            <prod:url>?</prod:url>
         </tem:product>
      </tem:PutProduct>
   </soapenv:Body>
</soapenv:Envelope>
```
PutProductContacts```
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:tem="http://tempuri.org/" xmlns:prod="http://www.example.com/product">
   <soapenv:Header/>
   <soapenv:Body>
      <tem:PutProductContacts>
         <tem:id>?</tem:id>
         <tem:supplier>
            <prod:email>?</prod:email>
            <prod:name>?</prod:name>
            <prod:number>?</prod:number>
            <prod:surname>?</prod:surname>
         </tem:supplier>
      </tem:PutProductContacts>
   </soapenv:Body>
</soapenv:Envelope>
```