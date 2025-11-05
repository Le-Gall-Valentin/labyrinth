using Labyrinth.Build;

namespace LabyrinthTest
{
    [TestFixture(Description = "Keymaster should handle arbitrary distributions of doors and key rooms")]
    public class KeymasterTest
    {
        [Test]
        public void DoorThenRoom_Matches_NoExceptionOnDispose()
        {
            Assert.DoesNotThrow(() =>
            {
                using var km = new Keymaster();
                var d = km.NewDoor();
                var r = km.NewKeyRoom(); 
            });
        }

        [Test]
        public void RoomThenDoor_Matches_NoExceptionOnDispose()
        {
            Assert.DoesNotThrow(() =>
            {
                using var km = new Keymaster();
                var r = km.NewKeyRoom();
                var d = km.NewDoor();
            });   
        }

        [Test]
        public void Alternating_DKDK_Matches_All()
        {
            Assert.DoesNotThrow(() =>
            {
                using var km = new Keymaster();
                km.NewDoor();
                km.NewKeyRoom();
                km.NewDoor();
                km.NewKeyRoom(); 
            });
        }

        [Test]
        public void Batch_DoorsThenRooms_AllMatch()
        {
            Assert.DoesNotThrow(() =>
            {
                using var km = new Keymaster();
                km.NewDoor();
                km.NewDoor();
                km.NewKeyRoom();
                km.NewKeyRoom();
            });
        }

        [Test]
        public void Batch_RoomsThenDoors_AllMatch()
        {
            Assert.DoesNotThrow(() =>
            {
                using var km = new Keymaster();
                km.NewKeyRoom();
                km.NewKeyRoom();
                km.NewDoor();
                km.NewDoor();
            });
            
        }

        [Test]
        public void ManyDoorsThenManyRooms_AllMatch()
        {
            Assert.DoesNotThrow(() =>
            {
                using var km = new Keymaster();
                for (int i = 0; i < 10; i++) km.NewDoor();
                for (int i = 0; i < 10; i++) km.NewKeyRoom();
            });
            
        }

        [Test]
        public void ManyRoomsThenManyDoors_AllMatch()
        {
            Assert.DoesNotThrow(() =>
            {
                using var km = new Keymaster();
                for (int i = 0; i < 10; i++) km.NewKeyRoom();
                for (int i = 0; i < 10; i++) km.NewDoor();
            });
        }

        [Test]
        public void Unmatched_LeftoverDoor_ThrowsOnDispose()
        {
            var km = new Keymaster();
            km.NewDoor();
            Assert.Throws<InvalidOperationException>(() => km.Dispose());
        }

        [Test]
        public void Unmatched_LeftoverRoom_ThrowsOnDispose()
        {
            var km = new Keymaster();
            km.NewKeyRoom();
            Assert.Throws<InvalidOperationException>(() => km.Dispose());
        }

        [Test]
        public void MixedInterleavings_SameCounts_NoException()
        {
            Assert.DoesNotThrow(() =>
            {
                using var km = new Keymaster();
                km.NewDoor();
                km.NewDoor();
                km.NewKeyRoom();
                km.NewDoor();
                km.NewKeyRoom();
                km.NewKeyRoom();
                km.NewDoor();
                km.NewKeyRoom();
                km.NewKeyRoom();
                km.NewDoor();
            });
        }
    }
}
