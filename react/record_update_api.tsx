import axios from "axios";
import { Record } from "../types/Record";

const API_URL = "https://localhost:5001/api/Records"; // Update URL if needed

// Fetch all records
export const getRecords = async (): Promise<Record[]> => {
  const response = await axios.get<Record[]>(API_URL);
  return response.data;
};

// Create a new record
export const createRecord = async (record: Omit<Record, "id" | "createdAt">): Promise<Record> => {
  const response = await axios.post<Record>(API_URL, record);
  return response.data;
};

// Update an existing record
export const updateRecord = async (id: number, record: Omit<Record, "id" | "createdAt">): Promise<Record> => {
  const response = await axios.put<Record>(`${API_URL}/${id}`, record);
  return response.data;
};

// Delete a record
export const deleteRecord = async (id: number): Promise<void> => {
  await axios.delete(`${API_URL}/${id}`);
};