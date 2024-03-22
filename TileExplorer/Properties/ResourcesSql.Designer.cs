﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TileExplorer.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ResourcesSql {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResourcesSql() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TileExplorer.Properties.ResourcesSql", typeof(ResourcesSql).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to data source={0};version=3;foreign keys=true;.
        /// </summary>
        internal static string ConnectionString {
            get {
                return ResourceManager.GetString("ConnectionString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE INDEX IF NOT EXISTS tracks_datetimestart_idx
        ///ON tracks(datetimestart ASC);.
        /// </summary>
        internal static string CreateIndexTracksDateTimeStart {
            get {
                return ResourceManager.GetString("CreateIndexTracksDateTimeStart", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE INDEX IF NOT EXISTS tracks_points_trackid_idx
        ///ON tracks_points (trackid);.
        /// </summary>
        internal static string CreateIndexTracksPointsTrackId {
            get {
                return ResourceManager.GetString("CreateIndexTracksPointsTrackId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE INDEX IF NOT EXISTS tracks_tiles_tileid_idx
        ///ON tracks_tiles(tileid);.
        /// </summary>
        internal static string CreateIndexTracksTilesTileId {
            get {
                return ResourceManager.GetString("CreateIndexTracksTilesTileId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE INDEX IF NOT EXISTS tracks_tiles_trackid_idx
        ///ON tracks_tiles(trackid);.
        /// </summary>
        internal static string CreateIndexTracksTilesTrackId {
            get {
                return ResourceManager.GetString("CreateIndexTracksTilesTrackId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE IF NOT EXISTS equipments (
        ///	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
        ///	text TEXT, brand TEXT, model TEXT
        ///);.
        /// </summary>
        internal static string CreateTableEquipments {
            get {
                return ResourceManager.GetString("CreateTableEquipments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE IF NOT EXISTS markers (
        ///    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
        ///    lat REAL NOT NULL,
        ///    lng REAL NOT NULL,
        ///    text TEXT,
        ///    istextvisible INTEGER,
        ///    offsetx INTEGER,
        ///    offsety INTEGER
        ///);.
        /// </summary>
        internal static string CreateTableMarkers {
            get {
                return ResourceManager.GetString("CreateTableMarkers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE IF NOT EXISTS tiles (
        ///	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
        ///	x INTEGER NOT NULL, y INTEGER NOT NULL,
        ///	UNIQUE(x, y)
        ///);.
        /// </summary>
        internal static string CreateTableTiles {
            get {
                return ResourceManager.GetString("CreateTableTiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE IF NOT EXISTS tracks (
        ///	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
        ///	text TEXT, datetimestart TEXT, datetimefinish TEXT,
        ///	distance REAL, equipmentid INTEGER
        ///);.
        /// </summary>
        internal static string CreateTableTracks {
            get {
                return ResourceManager.GetString("CreateTableTracks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE IF NOT EXISTS tracks_points (
        ///	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
        ///	trackid INTEGER,
        ///	num INTEGER,
        ///	lat REAL NOT NULL, lng REAL NOT NULL,
        ///	datetime TEXT,
        ///	ele REAL,
        ///	distance REAL,
        ///	showonmap INTEGER,
        ///	FOREIGN KEY (trackid) REFERENCES tracks (id)
        ///	ON DELETE CASCADE
        ///	ON UPDATE CASCADE
        ///);.
        /// </summary>
        internal static string CreateTableTracksPoints {
            get {
                return ResourceManager.GetString("CreateTableTracksPoints", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE IF NOT EXISTS tracks_tiles (
        ///	id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
        ///	trackid INTEGER, tileid INTEGER,
        ///	FOREIGN KEY (trackid) REFERENCES tracks (id)
        ///	ON DELETE CASCADE
        ///	ON UPDATE CASCADE
        ///);.
        /// </summary>
        internal static string CreateTableTracksTiles {
            get {
                return ResourceManager.GetString("CreateTableTracksTiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TRIGGER IF NOT EXISTS tracks_tiles_ad
        ///AFTER DELETE ON tracks_tiles
        ///WHEN
        ///	(SELECT COUNT(*) FROM tracks_tiles WHERE tileid=OLD.tileid) = 0
        ///BEGIN
        ///	DELETE FROM tiles WHERE id=OLD.tileid;
        ///END;.
        /// </summary>
        internal static string CreateTriggerTracksTilesAD {
            get {
                return ResourceManager.GetString("CreateTriggerTracksTilesAD", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM equipments
        ///ORDER BY text;.
        /// </summary>
        internal static string SelectEquipments {
            get {
                return ResourceManager.GetString("SelectEquipments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  SELECT * FROM {0} WHERE id = :id;.
        /// </summary>
        internal static string SelectListItemById {
            get {
                return ResourceManager.GetString("SelectListItemById", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM markers ORDER BY text;.
        /// </summary>
        internal static string SelectMarkers {
            get {
                return ResourceManager.GetString("SelectMarkers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT text, count, distancesum, durationsum FROM
        ///(
        ///	SELECT equipmentid, COUNT(*) AS count, SUM(distance) / 1000 AS distancesum, SUM(JULIANDAY(datetimefinish) - JULIANDAY(datetimestart)) as durationsum,
        ///		CASE WHEN equipmentid = 0 THEN 2 ELSE 1 END AS type
        ///	FROM tracks GROUP BY equipmentid
        ///)
        ///LEFT JOIN equipments
        ///ON equipments.id = equipmentid
        ///ORDER BY type, text;.
        /// </summary>
        internal static string SelectResultEquipments {
            get {
                return ResourceManager.GetString("SelectResultEquipments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT (CASE WHEN year = 32000 THEN NULL ELSE year END) AS year, count, distancesum, durationsum
        ///FROM
        ///(SELECT 
        ///    CAST(STRFTIME(&apos;%Y&apos;, datetimestart) AS INTEGER) AS year,
        ///    COUNT(*) AS count,
        ///    SUM(distance) / 1000.0 AS distancesum, 
        ///	SUM(JULIANDAY(datetimefinish) - JULIANDAY(datetimestart)) as durationsum
        ///FROM
        ///    tracks
        ///GROUP BY year
        ///	UNION
        ///SELECT 
        ///    32000 AS year,
        ///    COUNT(*) AS count,
        ///    SUM(distance) / 1000.0 AS distancesum,
        ///	SUM(JULIANDAY(datetimefinish) - JULIANDAY(datetimestar [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SelectResultYears {
            get {
                return ResourceManager.GetString("SelectResultYears", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT id FROM tiles WHERE x = :x AND y = :y;.
        /// </summary>
        internal static string SelectTileIdByXY {
            get {
                return ResourceManager.GetString("SelectTileIdByXY", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM tiles;.
        /// </summary>
        internal static string SelectTiles {
            get {
                return ResourceManager.GetString("SelectTiles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM tiles
        ///WHERE id IN (
        ///	SELECT tileid FROM tracks_tiles
        ///	WHERE trackid = :trackid
        ///);.
        /// </summary>
        internal static string SelectTilesByTrackId {
            get {
                return ResourceManager.GetString("SelectTilesByTrackId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM tiles
        ///WHERE id IN (
        ///	SELECT tileid FROM tracks_tiles
        ///	WHERE trackid IN (
        ///		SELECT id FROM tracks
        ///		{0}
        ///	)
        ///);.
        /// </summary>
        internal static string SelectTilesByTrackIds {
            get {
                return ResourceManager.GetString("SelectTilesByTrackIds", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM tracks_points WHERE trackid = :trackid and showonmap ORDER BY num;.
        /// </summary>
        internal static string SelectTrackPointsByTrackId {
            get {
                return ResourceManager.GetString("SelectTrackPointsByTrackId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM tracks_points WHERE trackid = :trackid ORDER BY num;.
        /// </summary>
        internal static string SelectTrackPointsByTrackIdFull {
            get {
                return ResourceManager.GetString("SelectTrackPointsByTrackIdFull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT id, text, dt AS datetimestart, datetimefinish, distance,
        ///	equipmentid, equipmenttext, equipmentbrand, equipmentmodel,
        ///	SUM(CASE WHEN e = 0 THEN 1 ELSE 0 END) AS newtilescount
        ///FROM (
        ///	SELECT *,
        ///		EXISTS(
        ///			SELECT tileid, datetimestart
        ///			FROM tracks 
        ///				LEFT JOIN tracks_tiles ON tracks.id = tracks_tiles.trackid
        ///			WHERE tileid = tid AND datetimestart &lt; dt
        ///		) AS e
        ///	FROM (
        ///		SELECT tracks.id AS id, tracks.text AS text,
        ///			datetimestart AS dt, datetimefinish,
        ///			distance, tileid AS tid,        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SelectTracks {
            get {
                return ResourceManager.GetString("SelectTracks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM tracks
        ///WHERE id IN (
        ///	SELECT trackid FROM tracks_tiles 
        ///	WHERE tileid = :tileid
        ///)
        ///ORDER BY datetimestart;.
        /// </summary>
        internal static string SelectTracksByTileId {
            get {
                return ResourceManager.GetString("SelectTracksByTileId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT count(*) AS count, sum(distance) AS distance
        ///FROM tracks
        ///{0};.
        /// </summary>
        internal static string SelectTracksInfo {
            get {
                return ResourceManager.GetString("SelectTracksInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM tracks;.
        /// </summary>
        internal static string SelectTracksOnly {
            get {
                return ResourceManager.GetString("SelectTracksOnly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT datetimestart, CAST(STRFTIME(&apos;%Y&apos;, datetimestart) AS INTEGER) AS year, CAST(STRFTIME(&apos;%m&apos;, datetimestart) AS INTEGER) AS month
        ///FROM tracks
        ///GROUP by year, month;.
        /// </summary>
        internal static string SelectTracksTree {
            get {
                return ResourceManager.GetString("SelectTracksTree", resourceCulture);
            }
        }
    }
}
