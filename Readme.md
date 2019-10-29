# Table of contents with IronPdf

In this sample project I'm trying to create a table of contents for a PDF document using IronPdf.

A chapter can contain of multiple pages, so the TOC can only be created afterwards. 
Over here I'm keeping track of how many pages are rendered per chapter and merge the `TableOfContents` items afterwards.