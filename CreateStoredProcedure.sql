-- Stored Procedure: GetCategoryTree
-- This stored procedure retrieves all categories and returns them in a flat structure
-- The tree structure is built in the application code

USE CategoryDB;
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetCategoryTree')
    DROP PROCEDURE GetCategoryTree;
GO

CREATE PROCEDURE GetCategoryTree
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        Id,
        Name,
        ParentId
    FROM Categories
    ORDER BY ParentId, Name;
END
GO

-- Note: This stored procedure returns a flat list of all categories.
-- The tree structure is built in the CategoryTreeSpService using LINQ ToLookup.
-- For a recursive tree built entirely in SQL, you could use a CTE (Common Table Expression),
-- but building in application code gives more flexibility and maintains consistency with the EF approach.

