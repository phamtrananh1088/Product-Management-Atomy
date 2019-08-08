Public Class Province
    Public Property ZipCode As String
    Public Property Province As String
    Public Property Country As String
    Public Property IsCapital As String
    Public Sub New(a As String, b As String, c As String, d As Boolean)
        Me.ZipCode = a
        Me.Province = b
        Me.Country = c
        Me.IsCapital = d
    End Sub
    Private Shared ListProvince As List(Of Province)
    Public Shared Function GetAllProvinces() As List(Of Province)
        If ListProvince Is Nothing Then
            ListProvince = New List(Of Province)
            ListProvince.Add(New Province("100000", "Hà Nội", "Việt Nam", True))
            ListProvince.Add(New Province("700000", "Thành phố Hồ Chí Minh", "Việt Nam", False))
            ListProvince.Add(New Province("880000", "An Giang", "Việt Nam", False))
            ListProvince.Add(New Province("790000", "Bà Rịa-Vũng Tàu", "Việt Nam", False))
            ListProvince.Add(New Province("960000", "Bạc Liêu", "Việt Nam", False))
            ListProvince.Add(New Province("260000", "Bắc Kạn", "Việt Nam", False))
            ListProvince.Add(New Province("230000", "Bắc Giang", "Việt Nam", False))
            ListProvince.Add(New Province("220000", "Bắc Ninh", "Việt Nam", False))
            ListProvince.Add(New Province("930000", "Bến Tre", "Việt Nam", False))
            ListProvince.Add(New Province("820000", "Bình Dương", "Việt Nam", False))
            ListProvince.Add(New Province("590000", "Bình Định", "Việt Nam", False))
            ListProvince.Add(New Province("830000", "Bình Phước", "Việt Nam", False))
            ListProvince.Add(New Province("800000", "Bình Thuận", "Việt Nam", False))
            ListProvince.Add(New Province("970000", "Cà Mau", "Việt Nam", False))
            ListProvince.Add(New Province("270000", "Cao Bằng", "Việt Nam", False))
            ListProvince.Add(New Province("900000", "Cần Thơ", "Việt Nam", False))
            ListProvince.Add(New Province("550000", "Đà Nẵng", "Việt Nam", False))
            ListProvince.Add(New Province("630000", "Đắk Lắk", "Việt Nam", False))
            ListProvince.Add(New Province("640000", "Đắk Nông", "Việt Nam", False))
            ListProvince.Add(New Province("380000", "Điện Biên", "Việt Nam", False))
            ListProvince.Add(New Province("810000", "Đồng Nai", "Việt Nam", False))
            ListProvince.Add(New Province("870000", "Đồng Tháp", "Việt Nam", False))
            ListProvince.Add(New Province("600000", "Gia Lai", "Việt Nam", False))
            ListProvince.Add(New Province("310000", "Hà Giang", "Việt Nam", False))
            ListProvince.Add(New Province("400000", "Hà Nam", "Việt Nam", False))
            ListProvince.Add(New Province("480000", "Hà Tĩnh", "25", False))
            ListProvince.Add(New Province("170000", "Hải Dương", "26", False))
            ListProvince.Add(New Province("180000", "Hải Phòng", "27", False))
            ListProvince.Add(New Province("910000", "Hậu Giang", "28", False))
            ListProvince.Add(New Province("350000", "Hòa Bình", "29", False))
            ListProvince.Add(New Province("160000", "Hưng Yên", "31", False))
            ListProvince.Add(New Province("650000", "Khánh Hoà", "32", False))
            ListProvince.Add(New Province("920000", "Kiên Giang", "33", False))
            ListProvince.Add(New Province("580000", "Kon Tum", "34", False))
            ListProvince.Add(New Province("390000", "Lai Châu", "35", False))
            ListProvince.Add(New Province("240000", "Lạng Sơn", "36", False))
            ListProvince.Add(New Province("330000", "Lào Cai", "37", False))
            ListProvince.Add(New Province("670000", "Lâm Đồng", "38", False))
            ListProvince.Add(New Province("850000", "Long An", "39", False))
            ListProvince.Add(New Province("420000", "Nam Định", "40", False))
            ListProvince.Add(New Province("460000", "Nghệ An", "41", False))
            ListProvince.Add(New Province("430000", "Ninh Bình", "42", False))
            ListProvince.Add(New Province("660000", "Ninh Thuận", "43", False))
            ListProvince.Add(New Province("290000", "Phú Thọ", "44", False))
            ListProvince.Add(New Province("620000", "Phú Yên", "45", False))
            ListProvince.Add(New Province("510000", "Quảng Bình", "46", False))
            ListProvince.Add(New Province("560000", "Quảng Nam", "47", False))
            ListProvince.Add(New Province("570000", "Quảng Ngãi", "48", False))
            ListProvince.Add(New Province("200000", "Quảng Ninh", "49", False))
            ListProvince.Add(New Province("520000", "Quảng Trị", "50", False))
            ListProvince.Add(New Province("950000", "Sóc Trăng", "51", False))
            ListProvince.Add(New Province("360000", "Sơn La", "52", False))
            ListProvince.Add(New Province("840000", "Tây Ninh", "53", False))
            ListProvince.Add(New Province("410000", "Thái Bình", "54", False))
            ListProvince.Add(New Province("250000", "Thái Nguyên", "55", False))
            ListProvince.Add(New Province("440000", "Thanh Hoá", "56", False))
            ListProvince.Add(New Province("530000", "Thừa Thiên-Huế", "57", False))
            ListProvince.Add(New Province("860000", "Tiền Giang", "58", False))
            ListProvince.Add(New Province("940000", "Trà Vinh", "59", False))
            ListProvince.Add(New Province("300000", "Tuyên Quang", "60", False))
            ListProvince.Add(New Province("890000", "Vĩnh Long", "61", False))
            ListProvince.Add(New Province("280000", "Vĩnh Phúc", "62", False))
            ListProvince.Add(New Province("320000", "Yên Bái", "63", False))


        End If

        Return ListProvince
    End Function

End Class