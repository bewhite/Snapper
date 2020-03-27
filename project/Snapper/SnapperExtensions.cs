﻿using System.Diagnostics;
using System.IO;
using Snapper.Core;

namespace Snapper
{
    public static class SnapperExtensions
    {
        /// <summary>
        ///     Compares the provided object with the stored snapshot.
        /// </summary>
        /// <param name="snapshot">The object to compare with the stored snapshot</param>
        public static void ShouldMatchSnapshot(this object snapshot)
        {
            var snapper = SnapperFactory.GetJsonSnapper();
            snapper.MatchSnapshot(snapshot);
        }

        /// <summary>
        ///     **EXPERIMENTAL**
        ///     <para>This method may be removed or changed in future versions.</para>
        ///     <para></para>
        ///     <para>
        ///     Compares the provided object with the stored snapshot.
        ///     It takes a <c>SnapshotId</c> for finer control of how and where the snapshot is stored.
        ///     </para>
        /// </summary>
        /// <param name="snapshot">The object to compare with the stored snapshot</param>
        /// <param name="snapshotId">Describes how and where the snapshot is stored</param>
        public static void ShouldMatchSnapshot(this object snapshot, SnapshotId snapshotId)
        {
            var snapper = SnapperFactory.GetJsonSnapper();
            snapper.MatchSnapshot(snapshot, snapshotId);
        }

        /// <summary>
        ///     **EXPERIMENTAL**
        ///     <para>This method may be removed or changed in future versions.</para>
        ///     <para></para>
        ///     <para>
        ///     Compares the provided object with the stored snapshot.
        ///     It generates a snapshot Id relative to the current directory
        ///     </para>
        /// </summary>
        /// <param name="snapshot">The object to compare with the stored snapshot</param>
        public static void ShouldMatchRelativeSnapshot(this object snapshot)
        {
            snapshot.ShouldMatchSnapshot(GetRelativeSnapshotId());
        }

        /// <summary>
        ///     Compares the provided object with the stored child snapshot.
        ///     Takes in a unique child name, best used in theory tests.
        /// </summary>
        /// <param name="snapshot">The object to compare with the stored child snapshot</param>
        /// <param name="childSnapshotName">The name of the child snapshot name. Must be unique per test.</param>
        public static void ShouldMatchChildSnapshot(this object snapshot, string childSnapshotName)
        {
            var snapper = SnapperFactory.GetJsonSnapper();
            snapper.MatchSnapshot(snapshot, childSnapshotName);
        }

        /// <summary>
        ///     Compares the provided object with an inline snapshot.
        ///     Use when snapshots are simple and small.
        /// </summary>
        /// <param name="snapshot">The object to compare with the inline snapshot</param>
        /// <param name="expectedSnapshot">The inline snapshot to which the object is compared with</param>
        public static void ShouldMatchInlineSnapshot(this object snapshot, object expectedSnapshot)
        {
            var snapper = SnapperFactory.GetJsonInlineSnapper(expectedSnapshot);
            snapper.MatchSnapshot(snapshot);
        }

        private static SnapshotId GetRelativeSnapshotId()
        {
            var caller = new StackTrace().GetFrame(2).GetMethod();

            return new SnapshotId(
                Path.Combine(Directory.GetCurrentDirectory(), "_snapshots"),
                caller.DeclaringType.Name,
                caller.Name
            );
        }
    }
}
