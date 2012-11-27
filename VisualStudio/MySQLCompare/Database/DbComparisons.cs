using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySQLCompare.Models;

namespace MySQLCompare.Database
{
    /// <summary>
    /// Performs all database comparison work between the left profile and right profile.
    /// </summary>
    class DbComparisons
    {
        /// <summary>
        /// Compares the schema (currently, by using field names only) of the left and right profiles,
        /// and 
        /// </summary>
        /// <param name="leftProfile"></param>
        /// <param name="rightProfile"></param>
        /// <param name="tableFoundDelegate">A delegate that is invoked whenever a table comparison has completed. Useful for tracking progress.</param>
        /// <returns>TableComparison objects that explain the differences found between both profiles.</returns>
        public static List<TableComparison> CompareSchema(Profile leftProfile, Profile rightProfile, Action<double, TableComparison> tableFoundDelegate)
        {
            List<TableComparison> tables = new List<TableComparison>();

            DbConnection leftConnection = new DbConnection(leftProfile.Host, leftProfile.Port, leftProfile.UserName, leftProfile.Password, leftProfile.Schema);
            DbConnection rightConnection = new DbConnection(rightProfile.Host, rightProfile.Port, rightProfile.UserName, rightProfile.Password, rightProfile.Schema);

            if (leftConnection.Open())
            {
                if (rightConnection.Open())
                {
                    List<string> leftTables = DbTableDescriptions.GetTables(leftConnection);
                    List<string> rightTables = DbTableDescriptions.GetTables(rightConnection);

                    int leftTableCount = leftTables.Count;
                    int currentTableCount = 0;

                    foreach (string leftTable in leftTables)
                    {
                        currentTableCount++;

                        double progress = ((double)currentTableCount / (double)leftTableCount) * 100;

                        // Look for this table in the collection of right tables
                        if (!rightTables.Contains(leftTable))
                        {
                            Table missingTable = new Table(leftTable);

                            // Get the properties of this table
                            List<TableProperty> missingProperties = DbTableDescriptions.GetFieldsForTable(leftTable, leftConnection);
                            missingTable.Properties = missingProperties;

                            missingTable.CreateStatement = DbTableDescriptions.GetCreateTableStatement(leftTable, leftConnection);

                            TableComparison tableComp = new TableComparison(missingTable, null);

                            // Use the supplied delegate to inform the consumer that we've found another table
                            tableFoundDelegate(progress, tableComp);

                            tables.Add(tableComp);
                        }
                        else
                        {
                            // Compare the properties of this table with those of the right table
                            List<TableProperty> leftProperties = DbTableDescriptions.GetFieldsForTable(leftTable, leftConnection);
                            List<TableProperty> rightProperties = DbTableDescriptions.GetFieldsForTable(leftTable, rightConnection); // Note: Not a typo. Using "leftTable" as a convenience, since they should be identical.
                            List<TableProperty> leftMissingProperties = new List<TableProperty>();
                            List<TableProperty> rightMissingProperties = new List<TableProperty>();

                            // Search for properties in the left table that are missing from the right
                            foreach (TableProperty prop in leftProperties)
                            {
                                bool propFound = false;

                                foreach (TableProperty rightProp in rightProperties)
                                {
                                    if (rightProp.Name == prop.Name)
                                    {
                                        propFound = true;
                                    }
                                }

                                if (!propFound)
                                {
                                    leftMissingProperties.Add(prop);
                                }
                            }

                            // Search for properties in the right table that are missing from the left
                            foreach (TableProperty prop in rightProperties)
                            {
                                bool propFound = false;

                                foreach (TableProperty leftProp in leftProperties)
                                {
                                    if (leftProp.Name == prop.Name)
                                    {
                                        propFound = true;
                                    }
                                }

                                if (!propFound)
                                {
                                    rightMissingProperties.Add(prop);
                                }
                            }

                            if (leftMissingProperties.Count > 0 || rightMissingProperties.Count > 0)
                            {
                                TableComparison tableComp = new TableComparison();

                                if (leftMissingProperties.Count > 0)
                                {
                                    Table tableWithMissingProps = new Table(leftTable);
                                    tableWithMissingProps.Properties = leftMissingProperties;

                                    tableComp.LeftTable = tableWithMissingProps;
                                }

                                if (rightMissingProperties.Count > 0)
                                {
                                    Table tableWithMissingProps = new Table(leftTable);
                                    tableWithMissingProps.Properties = rightMissingProperties;

                                    tableComp.RightTable = tableWithMissingProps;
                                }

                                tables.Add(tableComp);

                                tableFoundDelegate(progress, tableComp);
                            }
                            else
                            {
                                tableFoundDelegate(progress, null);
                            }
                        }
                    }

                    rightConnection.Close();
                }
                else
                {
                    throw new Exception("Right connection error.");
                }

                leftConnection.Close();
            }
            else
            {
                throw new Exception("Left connection error.");
            }

            return tables;
        }
    }
}
