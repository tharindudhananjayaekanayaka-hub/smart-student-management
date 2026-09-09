# Smart Student Management System - Backend Setup Guide

මෙම Repository එක ඔබගේ පරිගණකයට Clone කර Backend API එක සාර්ථකව run කරගන්නා ආකාරය පිළිබඳ සම්පූර්ණ පියවරෙන් පියවර උපදෙස් මාලාව (Step-by-Step Guide).

---

## 📋 1. අවශ්‍ය මෘදුකාංග (Prerequisites)

Backend එක run කිරීමට පෙර ඔබගේ පරිගණකයේ පහත දෑ ස්ථාපනය (Install) කර තිබිය යුතුය:

1. **.NET 8.0 SDK**
   - Download: [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
   - පරීක්ෂා කිරීමට: Terminal එකේ `dotnet --version` ගසන්න (අවම වශයෙන් `8.0.xxx` විය යුතුය).

2. **Git**
   - Download: [https://git-scm.com/downloads](https://git-scm.com/downloads)

3. **Code Editor / IDE**
   - **VS Code** (C# Dev Kit extension එක සමඟ) හෝ **Visual Studio 2022**

4. **Entity Framework Core CLI Tool (dotnet-ef)**
   - Terminal එක open කර පහත command එක run කරන්න:
     ```bash
     dotnet tool install --global dotnet-ef
     ```
   - (දැනටමත් install කර ඇත්නම් update කිරීමට: `dotnet tool update --global dotnet-ef`)

---

## 🚀 2. Clone කර Project එක Run කිරීම

### පියවර 1: Repository එක Clone කරගන්න
ඔබ කැමති Folder එකක් තුළ Terminal / PowerShell open කර පහත command එක run කරන්න:

```bash
git clone https://github.com/tharindudhananjayaekanayaka-hub/smart-student-management.git
```

### පියවර 2: API Project Folder එකට යන්න
```bash
cd smart-student-management/backend/StudentManagement.API
```

### පියවර 3: Dependencies Restore කර Build කරන්න
```bash
dotnet restore
dotnet build
```
*(Build succeeded - 0 Error(s) ලැබිය යුතුය)*

### පියවර 4: Database Update කිරීම (Neon PostgreSQL)
Cloud Database (Neon PostgreSQL) එකට migrations apply කරගැනීමට:
```bash
dotnet ef database update
```
> **සටහන:** Database එක Cloud (Neon.tech) හි host කර ඇති බැවින් Database tables සහ data සියලු දෙනා අතරේම share වේ. අමතර local PostgreSQL setup එකක් අවශ්‍ය නොවේ.

### පියවර 5: Backend එක Run කිරීම
```bash
dotnet run
```

Run වූ පසු Terminal එකේ පහත ආකාරයේ message එකක් පෙන්වයි:
```text
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5013
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

---

## 🌐 3. Swagger UI හරහා Endpoints පරීක්ෂා කිරීම

ඔබගේ Browser එක (Chrome / Edge) open කර පහත Link එකට පිවිසෙන්න:

👉 **[http://localhost:5013/swagger](http://localhost:5013/swagger)**  
*(හෝ `http://localhost:5013`)*

---

## 🧪 4. Testing Endpoints (පියවරෙන් පියවර)

### A. ශිෂ්‍යයෙක් Register කිරීම (`POST /api/Auth/register`)
1. Swagger හි `POST /api/Auth/register` ක්ලික් කර **Try it out** ඔබන්න.
2. පහත JSON එක paste කර **Execute** කරන්න:
```json
{
  "fullName": "Kamal Perera",
  "email": "kamal@sliit.lk",
  "password": "Password@123",
  "role": "Student",
  "studentRegNumber": "IT24100001"
}
```
3. **Response:** `200 OK` - `{"message": "User registered successfully!"}`

---

### B. Login වී JWT Token ලබා ගැනීම (`POST /api/Auth/login`)
1. `POST /api/Auth/login` ක්ලික් කර **Try it out** ඔබන්න.
2. Register වූ විස්තර ඇතුළත් කර **Execute** කරන්න:
```json
{
  "email": "kamal@sliit.lk",
  "password": "Password@123"
}
```
3. Response එකෙන් ලැබෙන `"token"` එකේ ඇති දිගු string එක **Copy** කරගන්න.

---

### C. Swagger හරහා Authorize කිරීම (Bearer Token)
1. Swagger පිටුවේ ඉහළ දකුණු කෙළවරේ ඇති කොළ පැහැති **Authorize 🔓** බොත්තම ක්ලික් කරන්න.
2. Value කොටුවේ පහත ආකාරයට type කරන්න (`Bearer` සහ token එක අතර space එකක් තබන්න):
   ```text
   Bearer <ඔබ_copy_කරගත්_token_අගය>
   ```
   *උදාහරණයක් ලෙස:* `Bearer eyJhbGciOiJIUzI1NiIsIn...`
3. **Authorize** ක්ලික් කර **Close** කරන්න (දැන් Icon එක 🔒 ලෙස පෙනෙනු ඇත).

---

### D. Protected Profile Endpoint එක Test කිරීම (`GET /api/User/profile`)
1. `GET /api/User/profile` ක්ලික් කර **Try it out** -> **Execute** ඔබන්න.
2. ඔබ සාර්ථකව Authorize වී ඇත්නම්, `200 OK` සමඟ අදාළ User & Student Profile විස්තර ලැබෙනු ඇත.

---

## ⚙️ 5. Configuration Settings (`appsettings.json`)

අවශ්‍ය නම් [appsettings.json](file:///D:/New%20folder/smart-student-management/backend/StudentManagement.API/appsettings.json) හි පහත configuration අඩංගු බව තහවුරු කරගන්න:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=ep-solitary-leaf-b341n6e4-pooler.c-4.ap-southeast-1.aws.neon.tech; Database=neondb; Username=neondb_owner; Password=npg_TtR59lDCxJYU; SSL Mode=VerifyFull; Channel Binding=Require;"
  },
  "Jwt": {
    "Key": "ThisIsASecretKeyForStudentManagementSystem2026SecureKey",
    "Issuer": "StudentManagementAPI",
    "Audience": "StudentManagementClient"
  }
}
```

---

## ⚠️ 6. නිතර ඇතිවිය හැකි ගැටළු සහ විසඳුම් (Troubleshooting)

| ගැටලුව | හේතුව | විසඳුම |
|---|---|---|
| `dotnet ef` command not found | EF CLI tool එක install කර නොතිබීම | `dotnet tool install --global dotnet-ef` run කරන්න |
| `Address already in use` (Port conflict) | කලින් run වූ process එකක් port 5013 අල්ලාගෙන සිටීම | Terminal එක close කර නැවත `dotnet run` කරන්න |
| `401 Unauthorized` on Profile endpoint | Token එක Authorize නොකිරීම හෝ `Bearer ` prefix එක අමතක වීම | `Bearer <token>` නිවැරදිව ලබා දී නැවත Authorize කරන්න |
| Database connection error | අන්තර්ජාල සම්බන්ධතාවය නොමැති වීම | Neon PostgreSQL Cloud Database එකක් බැවින් Internet Connection එක පරීක්ෂා කරන්න |
