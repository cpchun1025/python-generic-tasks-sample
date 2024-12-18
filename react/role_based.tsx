import React, { createContext, useContext, useState, ReactNode } from "react";

// Define the User type
type User = {
  name: string;
  role: "user" | "admin"; // Define specific roles
};

// Define the context type
type UserContextType = {
  user: User;
  setUser: React.Dispatch<React.SetStateAction<User>>;
};

// Create a context with a default value
const UserContext = createContext<UserContextType | undefined>(undefined);

// Define props for the UserProvider component
type UserProviderProps = {
  children: ReactNode; // ReactNode allows any valid React children
};

// UserProvider component to provide user state
const UserProvider: React.FC<UserProviderProps> = ({ children }) => {
  // Example user state (can be fetched from an API)
  const [user, setUser] = useState<User>({
    name: "John Doe",
    role: "user", // Change to 'admin' to test admin role
  });

  return (
    <UserContext.Provider value={{ user, setUser }}>
      {children}
    </UserContext.Provider>
  );
};

// Custom hook to use the UserContext
const useUser = (): UserContextType => {
  const context = useContext(UserContext);
  if (!context) {
    throw new Error("useUser must be used within a UserProvider");
  }
  return context;
};

// Navigation component
const Navigation: React.FC = () => {
  const { user } = useUser();

  return (
    <nav>
      <ul>
        <li>
          <a href="/home">Home</a>
        </li>
        <li>
          <a href="/profile">Profile</a>
        </li>

        {/* Conditionally render admin link based on user role */}
        {user.role === "admin" && (
          <li>
            <a href="/admin">Admin Panel</a>
          </li>
        )}
      </ul>
    </nav>
  );
};

// Reusable RequireRole component
type RequireRoleProps = {
  role: "user" | "admin";
  children: ReactNode;
};

const RequireRole: React.FC<RequireRoleProps> = ({ role, children }) => {
  const { user } = useUser();

  // Render children only if the user's role matches the required role
  return user.role === role ? <>{children}</> : null;
};

// Main App component
const App: React.FC = () => {
  return (
    <UserProvider>
      <Navigation />
      <div>
        <h1>Welcome to the App!</h1>
      </div>
    </UserProvider>
  );
};

export default App;