//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using DndTable.Core;
//using DndTable.Core.Characters;
//using DndTable.Core.Entities;
//using UnityEngine;
//using Object = UnityEngine.Object;

//namespace DndTable.UnityUI
//{
//    public class Factory
//    {
//        private IGame _game;
//        private MonoBehaviour _root;

//        public Transform PlayerTemplate { get; set; }
//        public Transform WallTemplate { get; set; }
//        public Transform TileTemplate { get; set; }

//        private void CreateBoard()
//        {
//            for (int i = 0; i < _game.GameBoard.MaxX; i++)
//            {
//                for (int j = 0; j < _game.GameBoard.MaxY; j++)
//                {
//                    CreateTile(i, j);

//                    var entity = _game.GameBoard.GetEntity(Position.Create(i, j));
//                    if (entity != null)
//                    {
//                        if (entity.EntityType == EntityTypeEnum.Character)
//                        {
//                            CreateEntity(PlayerTemplate, i, j, entity);
//                        }
//                        else if (entity.EntityType == EntityTypeEnum.Wall)
//                        {
//                            CreateEntity(WallTemplate, i, j, entity);
//                        }
//                        else
//                        {
//                            throw new NotSupportedException("EntityType does not have a template Transform: " + entity.EntityType);
//                        }
//                    }
//                }
//            }
//        }

//        private void CreateTile(int i, int j)
//        {
//            Vector3 position = new Vector3(i, 0, j);
//            Transform newObj = (Transform)Object.Instantiate(TileTemplate, position, Quaternion.identity);

//            // Set as child
//            newObj.transform.parent = _root.transform;
//        }

//        private void CreateEntity(Transform template, int i, int j, IEntity entity)
//        {
//            Vector3 position = new Vector3(i, 0, j);
//            Transform newObj = (Transform)Object.Instantiate(template, position, Quaternion.identity);

//            // Set as child
//            newObj.transform.parent = _root.transform;

//            // Entity
//            {
//                var entityScript = newObj.GetComponent("EntityScript") as EntityScript;
//                if (entityScript != null)
//                {
//                    entityScript.Entity = entity;
//                    entityScript.Game = _game;
//                }
//            }

//            // CharacterSheetInfo
//            if (entity is ICharacter)
//            {
//                var characterSheetInfoScript = newObj.GetComponent("CharacterSheetInfo") as CharacterSheetInfo;
//                if (characterSheetInfoScript != null)
//                {
//                    characterSheetInfoScript.Character = entity as ICharacter;
//                }
//            }

//        }
//    }
//}
