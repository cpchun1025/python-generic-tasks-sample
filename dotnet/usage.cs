ExcelProcessor.ProcessExcel("input.xlsx", "output.xlsx");
await EmailSender.SendEmailWithAttachment("output.xlsx", "recipient@example.com");