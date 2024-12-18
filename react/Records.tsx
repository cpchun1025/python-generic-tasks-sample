import React, { useState, useEffect } from "react";
import { Record } from "../types/Record";
import { getRecords, createRecord, updateRecord, deleteRecord } from "../services/api";

const Records: React.FC = () => {
  const [records, setRecords] = useState<Record[]>([]);
  const [newRecord, setNewRecord] = useState<{ name: string; description: string }>({
    name: "",
    description: "",
  });
  const [editRecord, setEditRecord] = useState<Record | null>(null);

  // Fetch records from the API
  useEffect(() => {
    fetchRecords();
  }, []);

  const fetchRecords = async () => {
    const data = await getRecords();
    setRecords(data);
  };

  const handleCreate = async () => {
    if (!newRecord.name || !newRecord.description) return;
    await createRecord(newRecord);
    setNewRecord({ name: "", description: "" });
    fetchRecords();
  };

  const handleEdit = async () => {
    if (!editRecord) return;
    await updateRecord(editRecord.id, {
      name: editRecord.name,
      description: editRecord.description,
    });
    setEditRecord(null);
    fetchRecords();
  };

  const handleDelete = async (id: number) => {
    await deleteRecord(id);
    fetchRecords();
  };

  return (
    <div>
      <h1>Records</h1>

      {/* New Record Form */}
      <div>
        <h2>Add a New Record</h2>
        <input
          type="text"
          placeholder="Name"
          value={newRecord.name}
          onChange={(e) => setNewRecord({ ...newRecord, name: e.target.value })}
        />
        <input
          type="text"
          placeholder="Description"
          value={newRecord.description}
          onChange={(e) => setNewRecord({ ...newRecord, description: e.target.value })}
        />
        <button onClick={handleCreate}>Add Record</button>
      </div>

      {/* Edit Record Form */}
      {editRecord && (
        <div>
          <h2>Edit Record</h2>
          <input
            type="text"
            placeholder="Name"
            value={editRecord.name}
            onChange={(e) => setEditRecord({ ...editRecord, name: e.target.value })}
          />
          <input
            type="text"
            placeholder="Description"
            value={editRecord.description}
            onChange={(e) => setEditRecord({ ...editRecord, description: e.target.value })}
          />
          <button onClick={handleEdit}>Save Changes</button>
        </div>
      )}

      {/* Records List */}
      <div>
        <h2>Record List</h2>
        <ul>
          {records.map((record) => (
            <li key={record.id}>
              <strong>{record.name}</strong>: {record.description} (Created At: {new Date(record.createdAt).toLocaleString()})
              <button onClick={() => setEditRecord(record)}>Edit</button>
              <button onClick={() => handleDelete(record.id)}>Delete</button>
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
};

export default Records;