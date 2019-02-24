using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZwSoft.ZwCAD.Runtime;
using ZwSoft.ZwCAD.ApplicationServices;
using ZwSoft.ZwCAD.DatabaseServices;
using ZwSoft.ZwCAD.EditorInput;
using ZwSoft.ZwCAD.Geometry;
using ZwSoft.ZwCAD.Colors;

[assembly: CommandClass(typeof(ClassLibrary1.Class1))]
  
namespace ClassLibrary1
{
    public class Class1
    {
        [CommandMethod("Shrinkage01")]
        public void CreatingABlock()
        {
            // Get the current database and start a transaction
            Document zwDoc = Application.DocumentManager.MdiActiveDocument;
            Database zwCurDb = zwDoc.Database;
            Editor ed = zwDoc.Editor;
            //Database zwCurDb;
            //zwCurDb = Application.DocumentManager.MdiActiveDocument.Database;

            TypedValue[] zwTypValAr = new TypedValue[1];
            zwTypValAr.SetValue(new TypedValue((int)DxfCode.Color, 256), 0);
            SelectionFilter zwSelFtr = new SelectionFilter(zwTypValAr);

            PromptDoubleOptions XValue = new PromptDoubleOptions("");
            XValue.Message = "\nEnter X scale: ";
            XValue.AllowZero = false;
            XValue.AllowNone = false;
            XValue.AllowNegative = false;
            PromptDoubleResult XValRes = ed.GetDouble(XValue);

            PromptDoubleOptions YValue = new PromptDoubleOptions("");
            YValue.Message = "\nEnter Y scale: ";
            YValue.AllowZero = false;
            YValue.AllowNone = false;
            YValue.AllowNegative = false;
            PromptDoubleResult YValRes = ed.GetDouble(YValue);

            PromptSelectionResult zwSSPrompt = zwDoc.Editor.GetSelection(zwSelFtr);

            using (Transaction zwTrans = zwCurDb.TransactionManager.StartTransaction())
            {
                //open the Layer Tabel for read
                LayerTable zwLyrTbl;
                zwLyrTbl = zwTrans.GetObject(zwCurDb.LayerTableId, OpenMode.ForRead) as LayerTable;

                if (zwLyrTbl.Has("D0-XYLogo"))
                {
                    string zwLayerName = "D0-XYLogo";
                    Application.ShowAlertDialog("Layer exist! Change layer success.");
                    zwCurDb.Clayer = zwLyrTbl[zwLayerName];
                }
                else
                {

                    string zwLayerName = "D0-XYLogo";
                    LayerTableRecord zwLyrTblRec = new LayerTableRecord();
                    zwLyrTblRec.Color = Color.FromColorIndex(ColorMethod.ByAci, 3);
                    zwLyrTblRec.Name = zwLayerName;
                    zwLyrTblRec.IsLocked = true;

                    zwLyrTbl.UpgradeOpen();
                    zwLyrTbl.Add(zwLyrTblRec);
                    zwTrans.AddNewlyCreatedDBObject(zwLyrTblRec, true);                    

                    zwCurDb.Clayer = zwLyrTbl[zwLayerName];
                    //Application.ShowAlertDialog("Layer does not exist!");

                }


                // Open the Block table for read
                BlockTable zwBlkTbl;
                zwBlkTbl = zwTrans.GetObject(zwCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                ObjectId blkRecId = ObjectId.Null;

                if (!zwBlkTbl.Has("DWG-XYLogo"))
                {
                    using (BlockTableRecord zwBlkTblRec = new BlockTableRecord())
                    {
                        zwBlkTblRec.Name = "DWG-XYLogo";

                        // Set the insertion point for the block
                        zwBlkTblRec.Origin = new Point3d(0, 0, 0);

                        // Add a circle to the block
                        // using (Circle acCirc = new Circle())
                        {
                            //Create Line
                            Line zwLine_1 = new Line(new Point3d(-1, -1, 0),
                                                    new Point3d(8, -1, 0));
                            Line zwLine_2 = new Line(new Point3d(8, -1, 0),
                                                    new Point3d(8, 1, 0));
                            Line zwLine_3 = new Line(new Point3d(8, 1, 0),
                                                    new Point3d(1, 1, 0));
                            Line zwLine_4 = new Line(new Point3d(1, 1, 0),
                                                    new Point3d(1, 8, 0));
                            Line zwLine_5 = new Line(new Point3d(1, 8, 0),
                                                    new Point3d(-1, 8, 0));
                            Line zwLine_6 = new Line(new Point3d(-1, 8, 0),
                                                    new Point3d(-1, -1, 0));
                            Line zwLine_7 = new Line(new Point3d(-1, -1, 0),
                                                    new Point3d(1, 1, 0));

                            //Arrow_Horizontal
                            Line zwLine_11 = new Line(new Point3d(8, 0, 0),
                                                    new Point3d(13, 0, 0));
                            Line zwLine_12 = new Line(new Point3d(13, 0, 0),
                                                    new Point3d(10, 0.8, 0));
                            Line zwLine_13 = new Line(new Point3d(13, 0, 0),
                                                    new Point3d(10, -0.8, 0));

                            //Arrow_Vertical
                            Line zwLine_21 = new Line(new Point3d(0, 8, 0),
                                                    new Point3d(0, 13, 0));
                            Line zwLine_22 = new Line(new Point3d(0, 13, 0),
                                                    new Point3d(-0.8, 10, 0));
                            Line zwLine_23 = new Line(new Point3d(0, 13, 0),
                                                    new Point3d(0.8, 10, 0));

                            //Create Text
                            DBText Text_X_Scale = new DBText();
                            Text_X_Scale.SetDatabaseDefaults();
                            Text_X_Scale.Position = new Point3d(1, -0.5, 0);
                            Text_X_Scale.Height = 0.7;
                            Text_X_Scale.TextString = string.Format("X = {0}", XValRes.Value.ToString());

                            DBText Text_Y_Scale = new DBText();
                            Text_Y_Scale.SetDatabaseDefaults();
                            Text_Y_Scale.Position = new Point3d(0.5, 1, 0);
                            Text_Y_Scale.Height = 0.7;
                            Text_Y_Scale.Rotation = 1.571;
                            Text_Y_Scale.TextString = string.Format("Y = {0}", YValRes.Value.ToString());

                            zwLine_1.SetDatabaseDefaults();
                            zwLine_2.SetDatabaseDefaults();
                            zwLine_3.SetDatabaseDefaults();
                            zwLine_4.SetDatabaseDefaults();
                            zwLine_5.SetDatabaseDefaults();
                            zwLine_6.SetDatabaseDefaults();
                            zwLine_7.SetDatabaseDefaults();

                            zwLine_11.SetDatabaseDefaults();
                            zwLine_12.SetDatabaseDefaults();
                            zwLine_13.SetDatabaseDefaults();

                            zwLine_21.SetDatabaseDefaults();
                            zwLine_22.SetDatabaseDefaults();
                            zwLine_23.SetDatabaseDefaults();

                            //Add object to Block Table record and the transaction
                            zwBlkTblRec.AppendEntity(Text_X_Scale);

                            zwBlkTblRec.AppendEntity(Text_Y_Scale);

                            zwBlkTblRec.AppendEntity(zwLine_1);
                            zwBlkTblRec.AppendEntity(zwLine_2);
                            zwBlkTblRec.AppendEntity(zwLine_3);
                            zwBlkTblRec.AppendEntity(zwLine_4);
                            zwBlkTblRec.AppendEntity(zwLine_5);
                            zwBlkTblRec.AppendEntity(zwLine_6);
                            zwBlkTblRec.AppendEntity(zwLine_7);
                            zwBlkTblRec.AppendEntity(zwLine_11);
                            zwBlkTblRec.AppendEntity(zwLine_12);
                            zwBlkTblRec.AppendEntity(zwLine_13);
                            zwBlkTblRec.AppendEntity(zwLine_21);
                            zwBlkTblRec.AppendEntity(zwLine_22);
                            zwBlkTblRec.AppendEntity(zwLine_23);
                            zwBlkTbl.UpgradeOpen();
                            zwBlkTbl.Add(zwBlkTblRec);
                            zwTrans.AddNewlyCreatedDBObject(zwBlkTblRec, true);

                            string zwLayer0 = "0";
                            zwCurDb.Clayer = zwLyrTbl[zwLayer0];
                            ; zwCurDb.Clayer = zwLyrTbl["Defpoints"];
                        }
                        blkRecId = zwBlkTblRec.Id;
                    }
                }
                else
                {
                    //blkRecId = zwBlkTbl["DWG-XYLogo"];
                    string zwLayer0 = "0";
                    zwCurDb.Clayer = zwLyrTbl[zwLayer0];
                    ed.WriteMessage("Block already exist. Program stop.");
                    Application.ShowAlertDialog("Block already exist. Program stop.");
                    //Environment.Exit(-1);
                    //Application.Quit();                    
                }

                if (blkRecId != ObjectId.Null)
                {
                    using (BlockReference zwBlkRef = new BlockReference(new Point3d(0, 0, 0), blkRecId))
                    {
                        BlockTableRecord zwCurSpaceBlkTblRec;
                        zwCurSpaceBlkTblRec = zwTrans.GetObject(zwCurDb.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                        zwCurSpaceBlkTblRec.AppendEntity(zwBlkRef);
                        zwTrans.AddNewlyCreatedDBObject(zwBlkRef, true);
                    }
                }

                //PromptSelectionResult zwSSPrompt = zwDoc.Editor.GetSelection(zwSelFtr);
                if (zwSSPrompt.Status == PromptStatus.OK)
                {
                    SelectionSet zwSSet = zwSSPrompt.Value;
                    //Application.ShowAlertDialog(zwSSet.Count.ToString());
                    IntegerCollection[] test = new IntegerCollection[zwSSet.Count];
                    BlockTable zwBlkTbl2;
                    zwBlkTbl2 = zwTrans.GetObject(zwCurDb.BlockTableId, OpenMode.ForWrite) as BlockTable;

                    BlockTableRecord zwBlkTblRec2;
                    zwBlkTblRec2 = zwTrans.GetObject(zwBlkTbl2[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                    foreach (var item in zwSSet)
                    {
                        CrossingOrWindowSelectedObject currentObject = item as CrossingOrWindowSelectedObject;
                        if (currentObject !=null)
                        {
                            try
                            {
                                BlockReference blockRef = zwTrans.GetObject(currentObject.ObjectId, OpenMode.ForWrite) as BlockReference;
                                if (blockRef != null)
                                {
                                    Point3d oldPoint = new Point3d(new double[] { blockRef.Position.X, blockRef.Position.Y, blockRef.Position.Z });
                                    Vector3d newVector = oldPoint.GetVectorTo(new Point3d(new double[] {blockRef.Position.X * 1.1, blockRef.Position.Y * 1.7, blockRef.Position.Z}));
                                    blockRef.TransformBy(Matrix3d.Displacement(newVector));
                                    Point3d newPoint = new Point3d(new double[]{blockRef.Position.X * 1.1, blockRef.Position.Y * 1.7, blockRef.Position.Z});
                                    zwTrans.AddNewlyCreatedDBObject(blockRef, true);
                                }
                            }
                            catch (System.Exception ex)
                            {
                                System.Diagnostics.Trace.WriteLine(ex);
                            }
                        }
                        else
                        {
                            Application.ShowAlertDialog("Empty");
                        }                        
                    }
                }



                // Save the new object to the database
                zwTrans.Commit();

                // Dispose of the transaction
            }
        }



    }
}
