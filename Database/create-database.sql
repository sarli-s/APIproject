-- ============================================================
-- WebAPIShop database: tables + starter data (5 categories, 20 products)
-- Generated from the EF Core model (Repositories/dbSHOPContext.cs).
--
-- Usage:
--   1. Create an empty database, e.g.  CREATE DATABASE API_SHOP;
--   2. Run this script inside it (SSMS, or: sqlcmd -S . -E -d API_SHOP -f 65001 -i create-database.sql)
--   3. Put that database name in ConnectionStrings:check
-- Product images (IMAGE_URL) are file names in the frontend's public/productsImages folder.
-- ============================================================
CREATE TABLE [Categories] (
    [CATEGORY_ID] int NOT NULL IDENTITY,
    [CATEGORY_NAME] varchar(50) NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([CATEGORY_ID])
);
GO


CREATE TABLE [RATING] (
    [RATING_ID] int NOT NULL IDENTITY,
    [HOST] nvarchar(50) NULL,
    [METHOD] nchar(10) NULL,
    [PATH] nvarchar(50) NULL,
    [REFERER] nvarchar(100) NULL,
    [USER_AGENT] nvarchar(max) NULL,
    [Record_Date] datetime NULL,
    CONSTRAINT [PK_RATING] PRIMARY KEY ([RATING_ID])
);
GO


CREATE TABLE [User] (
    [userId] int NOT NULL IDENTITY,
    [userEmail] varchar(50) NOT NULL,
    [userFirstName] varchar(50) NULL,
    [userLastName] varchar(50) NULL,
    [userPassword] varchar(60) NOT NULL,
    [City] varchar(50) NULL,
    [Address] varchar(100) NULL,
    [Phon] varchar(20) NULL,
    [Role] varchar(20) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY ([userId])
);
GO


CREATE TABLE [Products] (
    [PRODUCT_ID] int NOT NULL IDENTITY,
    [PRODUCT_NAME] varchar(50) NOT NULL,
    [PRICE] float NOT NULL,
    [CATEGORY_ID] int NOT NULL,
    [DESCRIPTION] varchar(max) NULL,
    [IMAGE_URL] nvarchar(500) NULL,
    [COLORS] varchar(200) NULL,
    [TOPTEXT] nvarchar(200) NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([PRODUCT_ID]),
    CONSTRAINT [FK_Products_Categories] FOREIGN KEY ([CATEGORY_ID]) REFERENCES [Categories] ([CATEGORY_ID])
);
GO


CREATE TABLE [Orders] (
    [ORDER_ID] int NOT NULL IDENTITY,
    [ORDER_DATE] date NOT NULL,
    [ORDER_SUM] float NOT NULL,
    [USER_ID] int NOT NULL,
    [STATUS] nvarchar(50) NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([ORDER_ID]),
    CONSTRAINT [FK_Orders_User] FOREIGN KEY ([USER_ID]) REFERENCES [User] ([userId]) ON DELETE CASCADE
);
GO


CREATE TABLE [Order_item] (
    [ORDER_ITEM_ID] int NOT NULL IDENTITY,
    [PRODUCT_ID] int NOT NULL,
    [ORDER_ID] int NOT NULL,
    [QUANTITY] int NOT NULL,
    [POPULARCOLORE] nvarchar(50) NULL,
    [CUSTOMTEXT] nvarchar(max) NULL,
    CONSTRAINT [PK_Order_item] PRIMARY KEY ([ORDER_ITEM_ID]),
    CONSTRAINT [FK_Order_item_Orders] FOREIGN KEY ([ORDER_ID]) REFERENCES [Orders] ([ORDER_ID]),
    CONSTRAINT [FK_Order_item_Products] FOREIGN KEY ([PRODUCT_ID]) REFERENCES [Products] ([PRODUCT_ID])
);
GO


CREATE INDEX [IX_Order_item_ORDER_ID] ON [Order_item] ([ORDER_ID]);
GO


CREATE INDEX [IX_Order_item_PRODUCT_ID] ON [Order_item] ([PRODUCT_ID]);
GO


CREATE INDEX [IX_Orders_USER_ID] ON [Orders] ([USER_ID]);
GO


CREATE INDEX [IX_Products_CATEGORY_ID] ON [Products] ([CATEGORY_ID]);
GO

-- ---------- Starter data ----------
SET IDENTITY_INSERT [Categories] ON;
INSERT INTO [Categories] ([CATEGORY_ID], [CATEGORY_NAME]) VALUES (1, N'לבית ולמטבח');
INSERT INTO [Categories] ([CATEGORY_ID], [CATEGORY_NAME]) VALUES (2, N'אירועים ומתנות');
INSERT INTO [Categories] ([CATEGORY_ID], [CATEGORY_NAME]) VALUES (3, N'ציוד משרדי');
INSERT INTO [Categories] ([CATEGORY_ID], [CATEGORY_NAME]) VALUES (4, N'טקסטיל וביגוד');
INSERT INTO [Categories] ([CATEGORY_ID], [CATEGORY_NAME]) VALUES (5, N'אקססוריז');
SET IDENTITY_INSERT [Categories] OFF;
SET IDENTITY_INSERT [Products] ON;
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (12, N'קרש חיתוך מעץ אלון יוקרתי', 149, 1, N'קרש חיתוך עמיד ואיכותי העשוי מעץ אלון מלא. מתנה מושלמת ואיכותית למטבח הביתי, לחגים, לחנוכת בית, לחובבי בישול ואירוח, וליצירת אווירה ביתית חמה.', N'Chopboard.jpg', N'עץ טבעי, אלון כהה, אגוז', N'המטבח של משפחת לוי');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (13, N'סט תחתיות לכוסות משעם', 45, 1, N'סט של 6 תחתיות טבעיות המונעות סימני רטיבות. פתרון עיצובי ופרקטי לאירוח, הגנה על שולחנות מפני כוסות שתייה חמה וקרה, ומתנה נהדרת לחגים ומעברי דירה.', N'cork.jpg', N'חום בהיר, טבעי', N'לחיים!');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (14, N'ספל מאג קרמי עם כיתוב מוזהב', 39, 1, N'ספל קפה קלאסי בגימור מט. ספל מאג מפנק, נעים ומושלם לשתייה חמה במזג אוויר קר, מתאים במיוחד לימי חורף גשומים, לתה, קפה, שוקו חם ופינוק ביתי.', N'cup.jpg', N'לבן, שחור מט, ורוד עתיק', N'בוקר טוב אהובה');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (15, N'שטיח כניסה לבית בעיצוב אישי', 89, 1, N'שטיח "ברוכים הבאים" עשוי סיבי קוקוס. שטיח כניסה מעוצב, מעניק תחושה מזמינה, ביתית, חמימה ונעימה לכל מי שנכנס לבית. מתנה קלאסית לחנוכת בית או זוג צעיר.', N'rug.png', N'חום סיב, בז''', N'כאן גרים בכיף');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (16, N'קופסת עץ ליין עם חריטה', 65, 2, N'קופסת אחסון מהודרת לבקבוק יין. מארז יוקרתי מעץ, מתנה מרשימה, אלגנטית ומכובדת לגברים, לאבות, לחגים, לאירועים רשמיים, לארוחות ערב חגיגיות וימי הולדת.', N'wine.png', N'עץ אורן, מהגוני', N'בציר טוב - 2024');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (17, N'בלון גז קריסטלי עם כיתוב', 55, 2, N'בלון גז קריסטלי הממולא בקונפטי צבעוני. פריט דקורטיבי ומיוחד ליצירת אווירה שמחה וחגיגית, מתנה מושלמת ומקורית לימי הולדת, חגיגות, מסיבות הפתעה ואירועים משמחים.', N'balloon.jpg', N'שקוף, זהב, כסף, רוז גולד', N'מזל טוב לנסיכה');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (18, N'פאזל עץ "סיבות למה אני אוהב אותך"', 110, 2, N'פאזל ייחודי שבו על כל חלק נחרטת סיבה... מתנה רומנטית, מרגשת ואישית במיוחד לבני זוג, לאישה, לבעל, ליום האהבה (ולנטיין), ימי נישואין או סתם להביע אהבה עמוקה.', N'Puzzle.jpg', N'עץ בהיר', N'הסיבות שבלב');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (19, N'נר ריחני בכלי זכוכית עם הקדשה', 49, 2, N'נר ריחני בכלי זכוכית, בריחוח וניל או לבנדר. מעניק תחושת רוגע, שלווה ואווירה רומנטית, נעימה וחמימה בחדר, מושלם לימי חורף קרים, הפגת מתחים ופינוק ביתי.', N'Candle.jpg', N'לבן שמנת, סגול לילך', N'רגע של שלווה');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (20, N'עט מתכת יוקרתי עם חריטת שם', 79, 3, N'עט נובע או עט כדורי בגימור כרום. עט מתכת יוקרתי עם חריטת שם, מתנה מקצועית וייצוגית למנהלים, לבוגרים, לסטודנטים, לציון הישגים, למשרד או לרגל עבודה חדשה.', N'pen.jpg', N'שחור, כסף, כחול כהה', N'בהצלחה בדרך החדשה');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (21, N'מחברת פרימיום עם כריכה קשה', 59, 3, N'מחברת דפים שורות עם כריכת דמוי עור. מחברת פרימיום עם גיגים ומחשבות, מתנה מעולה לסטודנטים, כתיבה אישית, תכנון מטרות, יומן אישי, סביבת עבודה מעוצבת וארגון.', N'notebook.jpg', N'חום יוקרתי, שחור, ירוק בקבוק', N'הגיגים ומחשבות');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (22, N'משטח לעכבר (Mouse Pad) ממותג', 35, 3, N'משטח ארגונומי רך המאפשר עבודה חלקה. משטח לעכבר ממותג, פתרון נוח, שימושי ופרקטי לעבודה יומיומית מול מחשב במשרד או בבית, מתנה נהדרת וזמינה לחברים לעבודה.', N'Mouse-pad.jpg', N'כחול נייבי, אפור גרפיט', N'עובד מצטיין');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (23, N'מעמד שולחני לטלפון מעץ', 45, 3, N'סטנד מעוצב לטלפון הנייד המאפשר צפייה נוחה. מעמד שולחני פרקטי ויציב לסמארטפון, מתנה שימושית ופונקציונלית ליומיום, למשרד, ללימודים או לצפייה בסרטונים בנוחות.', N'Stand.jpg', N'עץ טבעי, אפור מודרני', N'תמיד מחובר');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (24, N'מגבת גוף עם רקמה אישית', 75, 4, N'מגבת רחצה גדולה ומפנקת עשויה 100% כותנה. מגבת גוף רכה ועוטפת, מעניקה תחושת ספא ביתי, מתנה מפנקת ואיכותית לאחרי המקלחת, לחגים, לחייל משתחרר או לזוגות.', N'Towel.jpg', N'לבן, תכלת, ורוד פודרה', N'אחרי המקלחת');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (25, N'ציפית לכרית עם משפט "לילה טוב"', 40, 4, N'ציפית רכה ונעימה למגע עליה ניתן להדפיס. ציפית לכרית עם כיתוב אישי, מתנה אינטימית, חמימה ומתוקה לחדר השינה, לילדים, לבני זוג, לאחות, או למישהו שאוהבים.', N'Pillowcase.jpg', N'לבן, אופ-ווייט', N'חלומות פז');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (26, N'חלוק רחצה מפנק עם רקמה', 180, 4, N'חלוק מגבת רך וסופג המעניק תחושת ספא. חלוק רחצה מפנק, עוטף ונעים במיוחד לימים קרירים ומזג אוויר קר, מתנה יוקרתית לאישה, לבעל, לימי הולדת ופינוק הגוף.', N'Towel-robe.jpg', N'לבן צחור, אפור מלאנג''', N'Spa Time');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (27, N'סינר מטבח בעיצוב אישי', 65, 4, N'סינר בד עמיד המגן על הבגדים. סינר מטבח מעוצב לבשלנים, מתנה מקורית ומשעשעת לחובבי אפייה ובישול, השף של הבית, ארוחות משפחתיות, ועל האש בקיץ.', N'Apron.jpg', N'שחור, אדום, דנים', N'השף של הבית');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (28, N'חולצת T-shirt עם כיתוב מקורי', 55, 5, N'חולצת כותנה בגזרה נוחה. חולצת T-shirt עם כיתוב מקורי, פריט לבוש קליל, יומיומי ונעים, מתנה כיפית, מגניבה ואישית לחברים, לקיץ, או לציון אירוע קבוצתי משעשע.', N'shirt.jpg', N'לבן, שחור, אפור', N'הכי מגניב בגן');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (29, N'כובע מצחייה (Cap) עם רקמה', 50, 5, N'כובע איכותי עם סגירה מתכווננת. כובע מצחייה בעיצוב קיצי (Cap) עם רקמה, מעניק הגנה מהשמש ומראה ספורטיבי ומגניב, מתנה נהדרת לצעירים, טיולים וימי הקיץ החמים.', N'hat.png', N'ץ', N'Summer Vibes');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (30, N'ארנק עור עם חריטה פנימית', 130, 5, N'ארנק גברים קלאסי מעור אמיתי. ארנק עור איכותי, יציב ואלגנטי, שומר על סדר, מתנה על-זמנית ומכובדת לגבר, לאבא, לבן זוג, לחייל, או ליום הולדת.', N'wallet.jpg', N'חום קוניאק, שחור', N'אוהבת תמיד');
INSERT INTO [Products] ([PRODUCT_ID], [PRODUCT_NAME], [PRICE], [CATEGORY_ID], [DESCRIPTION], [IMAGE_URL], [COLORS], [TOPTEXT]) VALUES (31, N'תיק בד (Tote Bag) ממותג', 30, 5, N'Eco-friendly reusable cloth bag. A light, useful and comfortable cloth bag (Tote bag) for carrying items, shopping and daily errands, a lovely, ecological and green gift with a message to protect the earth.', N'bag.jpg', N'בז'' טבעי, שחור', N'Save the Planet');
SET IDENTITY_INSERT [Products] OFF;
GO
