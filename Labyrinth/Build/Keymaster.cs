using Labyrinth.Items;
using Labyrinth.Tiles;

namespace Labyrinth.Build
{
    /// <summary>
    /// Manage the creation of doors and key rooms ensuring each door has a corresponding key room.
    /// </summary>
    public sealed class Keymaster : IDisposable
    {
        /// <summary>
        /// Ensure all created doors have a corresponding key room and vice versa.
        /// </summary>
        /// <exception cref="InvalidOperationException">Some keys are missing or are not placed.</exception>
        public void Dispose()
        {
            if (_unplacedKeys.Count > 0 || _emptyKeyRooms.Count > 0)
            {
                throw new InvalidOperationException("Unmatched key/door creation");
            }
        }

        /// <summary>
        /// Create a new door and place its key in a previously created empty key room (if any).
        /// </summary>
        /// <returns>Created door</returns>
        public Door NewDoor()
        {
            var door = new Door();
            
            var keyInv = new MyInventory();
            
            door.LockAndTakeKey(keyInv);
            
            _unplacedKeys.Enqueue(keyInv);
            
            PlaceKeysIfPossible();
            return door;
        }

        /// <summary>
        /// Create a new room with key and place the key if a door was previously created.
        /// </summary>
        /// <returns>Created key room</returns>
        public Room NewKeyRoom()
        {
            var room = new Room();
            _emptyKeyRooms.Enqueue(room);
            
            PlaceKeysIfPossible();
            return room;
        }

        private void PlaceKeysIfPossible()
        {
            while (_unplacedKeys.Count > 0 && _emptyKeyRooms.Count > 0)
            {
                var keyInv = _unplacedKeys.Peek();
                var room   = _emptyKeyRooms.Peek();
                
                room.Pass().MoveItemFrom(keyInv);
                
                _unplacedKeys.Dequeue();
                _emptyKeyRooms.Dequeue();
            }
        }
        
        private readonly Queue<MyInventory> _unplacedKeys = new();
        private readonly Queue<Room> _emptyKeyRooms = new();
    }
}