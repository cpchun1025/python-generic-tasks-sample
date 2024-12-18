import { useEffect } from "react";

const UserProvider: React.FC<UserProviderProps> = ({ children }) => {
  const [user, setUser] = useState<User | null>(null);

  useEffect(() => {
    const fetchUser = async () => {
      // Simulate an API call
      const userData: User = await fetch("/api/user").then((res) => res.json());
      setUser(userData);
    };

    fetchUser();
  }, []);

  if (!user) {
    return <div>Loading...</div>; // Show a loading state
  }

  return (
    <UserContext.Provider value={{ user, setUser }}>
      {children}
    </UserContext.Provider>
  );
};