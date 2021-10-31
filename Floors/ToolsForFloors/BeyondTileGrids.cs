using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotsMod
{
    class BeyondTileGrids
    {
        public static TileIndexGrid floorBorderGrid;
        public static List<float> timeSaver = new List<float> { 1 };

        public static void Init()
        {
            floorBorderGrid = new TileIndexGrid
            {
                topLeftIndices = new TileIndexList
                {
                    indexWeights = timeSaver,
                    indices = new List<int> { 41 }
                },
                topIndices = new TileIndexList
                {
                    indexWeights = timeSaver,
                    indices = new List<int> { 42 }
                },
                topRightIndices = new TileIndexList
                {
                    indexWeights = timeSaver,
                    indices = new List<int> { 43 }
                },
                leftIndices = new TileIndexList
                {
                    indexWeights = timeSaver,
                    indices = new List<int> { 51 }
                },
                rightIndices = new TileIndexList
                {
                    indexWeights = timeSaver,
                    indices = new List<int> { 53 }
                },
                bottomLeftIndices = new TileIndexList
                {
                    indexWeights = timeSaver,
                    indices = new List<int> { 61 }
                },
                bottomIndices = new TileIndexList
                {
                    indexWeights = timeSaver,
                    indices = new List<int> { 52 }
                },
                bottomRightIndices = new TileIndexList
                {
                    indexWeights = timeSaver,
                    indices = new List<int> { 63 }
                },
                topLeftNubIndices = new TileIndexList
                {
                    indexWeights = timeSaver,
                    indices = new List<int> { 65 }
                },
                topRightNubIndices = new TileIndexList
                {
                    indexWeights = timeSaver,
                    indices = new List<int> { 64 }
                },
                bottomLeftNubIndices = new TileIndexList
                {
                    indexWeights = timeSaver,
                    indices = new List<int> { 55 }
                },
                bottomRightNubIndices = new TileIndexList
                {
                    indexWeights = timeSaver,
                    indices = new List<int> { 54 }
                },
                diagonalNubsTopRightBottomLeft = new TileIndexList
                {
                    indexWeights = timeSaver,
                    indices = new List<int> { 114 }
                },
                diagonalNubsTopLeftBottomRight = new TileIndexList
                {
                    indexWeights = timeSaver,
                    indices = new List<int> { 113 }
                },
                quadNubs = new TileIndexList
                {
                    indexWeights = timeSaver,
                    indices = new List<int> { 115 }
                },

                CenterCheckerboard = false,
                CheckerboardDimension = 1,
                CenterIndicesAreStrata = true,
                PitInternalSquareGrids = new List<TileIndexGrid>(),
                PitInternalSquareOptions = new PitSquarePlacementOptions
                {
                    CanBeFlushBottom = false,
                    CanBeFlushLeft = false,
                    CanBeFlushRight = false,
                    PitSquareChance = 0.1f,
                },
                PitBorderIsInternal = false,
                PitBorderOverridesFloorTile = false,
                CeilingBorderUsesDistancedCenters = false,
                UsesRatChunkBorders = false,
                RatChunkBottomSet = new TileIndexList
                {
                    indexWeights = new List<float>(),
                    indices = new List<int>()
                }

            };
        }
    }
}
