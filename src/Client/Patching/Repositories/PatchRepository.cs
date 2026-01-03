// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Rapture.Client.Patching.Models;
using System.Collections.ObjectModel;

namespace Rapture.Client.Patching.Repositories;

/// <summary>
/// Provides access to the collection of available patch metadata entries for the game.
/// </summary>
public class PatchRepository
{
    /// <summary>
    /// Gets the collection of patch metadata entries available for the game.
    /// </summary>
    public ReadOnlyCollection<PatchInfo> PatchInfo { get; init; } =
    [
        new() { Platform = "win32", Channel = "release", Type = "boot", Version = "2010.07.10.0000", BuildTime = new(2010, 07, 10), RepositoryHash = "2d2a390f", FileSize = 0  },
        new() { Platform = "win32", Channel = "release", Type = "boot", Version = "2010.09.18.0000", BuildTime = new(2010, 09, 18), RepositoryHash = "2d2a390f", FileSize = 5571687  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2010.07.10.0000", BuildTime = new(2010, 07, 10), RepositoryHash = "48eca647", FileSize = 0  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2010.09.19.0000", BuildTime = new(2010, 09, 19), RepositoryHash = "48eca647", FileSize = 444398866  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2010.09.23.0000", BuildTime = new(2010, 09, 23), RepositoryHash = "48eca647", FileSize = 6907277  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2010.09.28.0000", BuildTime = new(2010, 09, 28), RepositoryHash = "48eca647", FileSize = 18803280  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2010.10.07.0001", BuildTime = new(2010, 10, 07), RepositoryHash = "48eca647", FileSize = 19226330  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2010.10.14.0000", BuildTime = new(2010, 10, 14), RepositoryHash = "48eca647", FileSize = 19464329  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2010.10.22.0000", BuildTime = new(2010, 10, 22), RepositoryHash = "48eca647", FileSize = 19778252  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2010.10.26.0000", BuildTime = new(2010, 10, 26), RepositoryHash = "48eca647", FileSize = 19778391  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2010.11.25.0002", BuildTime = new(2010, 11, 25), RepositoryHash = "48eca647", FileSize = 250718651  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2010.11.30.0000", BuildTime = new(2010, 11, 30), RepositoryHash = "48eca647", FileSize = 6921623  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2010.12.06.0000", BuildTime = new(2010, 12, 06), RepositoryHash = "48eca647", FileSize = 7158904  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2010.12.13.0000", BuildTime = new(2010, 12, 13), RepositoryHash = "48eca647", FileSize = 263311481  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2010.12.21.0000", BuildTime = new(2010, 12, 21), RepositoryHash = "48eca647", FileSize = 7521358  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.01.18.0000", BuildTime = new(2011, 01, 18), RepositoryHash = "48eca647", FileSize = 9954265  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.02.01.0000", BuildTime = new(2011, 02, 01), RepositoryHash = "48eca647", FileSize = 11632816  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.02.10.0000", BuildTime = new(2011, 02, 10), RepositoryHash = "48eca647", FileSize = 11714096  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.03.01.0000", BuildTime = new(2011, 03, 01), RepositoryHash = "48eca647", FileSize = 77464101  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.03.24.0000", BuildTime = new(2011, 03, 24), RepositoryHash = "48eca647", FileSize = 108923937  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.03.30.0000", BuildTime = new(2011, 03, 30), RepositoryHash = "48eca647", FileSize = 109010880  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.04.13.0000", BuildTime = new(2011, 04, 13), RepositoryHash = "48eca647", FileSize = 341603850  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.04.21.0000", BuildTime = new(2011, 04, 21), RepositoryHash = "48eca647", FileSize = 343579198  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.05.19.0000", BuildTime = new(2011, 05, 19), RepositoryHash = "48eca647", FileSize = 344239925  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.06.10.0000", BuildTime = new(2011, 06, 10), RepositoryHash = "48eca647", FileSize = 344334860  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.07.20.0000", BuildTime = new(2011, 07, 20), RepositoryHash = "48eca647", FileSize = 584926805  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.07.26.0000", BuildTime = new(2011, 07, 26), RepositoryHash = "48eca647", FileSize = 7649141  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.08.05.0000", BuildTime = new(2011, 08, 05), RepositoryHash = "48eca647", FileSize = 152064532  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.08.09.0000", BuildTime = new(2011, 08, 09), RepositoryHash = "48eca647", FileSize = 8573687  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.08.16.0000", BuildTime = new(2011, 08, 16), RepositoryHash = "48eca647", FileSize = 6118907  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.10.04.0000", BuildTime = new(2011, 10, 04), RepositoryHash = "48eca647", FileSize = 677633296  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.10.12.0001", BuildTime = new(2011, 10, 12), RepositoryHash = "48eca647", FileSize = 28941655  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.10.27.0000", BuildTime = new(2011, 10, 27), RepositoryHash = "48eca647", FileSize = 29179764  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.12.14.0000", BuildTime = new(2011, 12, 14), RepositoryHash = "48eca647", FileSize = 374617428  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2011.12.23.0000", BuildTime = new(2011, 12, 23), RepositoryHash = "48eca647", FileSize = 22363713  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.01.18.0000", BuildTime = new(2012, 01, 18), RepositoryHash = "48eca647", FileSize = 48998794  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.01.24.0000", BuildTime = new(2012, 01, 24), RepositoryHash = "48eca647", FileSize = 49126606  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.01.31.0000", BuildTime = new(2012, 01, 31), RepositoryHash = "48eca647", FileSize = 49536396  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.03.07.0000", BuildTime = new(2012, 03, 07), RepositoryHash = "48eca647", FileSize = 320630782  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.03.09.0000", BuildTime = new(2012, 03, 09), RepositoryHash = "48eca647", FileSize = 8312819  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.03.22.0000", BuildTime = new(2012, 03, 22), RepositoryHash = "48eca647", FileSize = 22027738  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.03.29.0000", BuildTime = new(2012, 03, 29), RepositoryHash = "48eca647", FileSize = 8322920  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.04.04.0000", BuildTime = new(2012, 04, 04), RepositoryHash = "48eca647", FileSize = 8678570  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.04.23.0001", BuildTime = new(2012, 04, 23), RepositoryHash = "48eca647", FileSize = 289511791  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.05.08.0000", BuildTime = new(2012, 05, 08), RepositoryHash = "48eca647", FileSize = 27266546  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.05.15.0000", BuildTime = new(2012, 05, 15), RepositoryHash = "48eca647", FileSize = 27416023  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.05.22.0000", BuildTime = new(2012, 05, 22), RepositoryHash = "48eca647", FileSize = 27742726  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.06.06.0000", BuildTime = new(2012, 06, 06), RepositoryHash = "48eca647", FileSize = 129984024  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.06.19.0000", BuildTime = new(2012, 06, 19), RepositoryHash = "48eca647", FileSize = 133434217  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.06.26.0000", BuildTime = new(2012, 06, 26), RepositoryHash = "48eca647", FileSize = 133581048  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.07.21.0000", BuildTime = new(2012, 07, 21), RepositoryHash = "48eca647", FileSize = 253224781  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.08.10.0000", BuildTime = new(2012, 08, 10), RepositoryHash = "48eca647", FileSize = 42851112  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.09.06.0000", BuildTime = new(2012, 09, 06), RepositoryHash = "48eca647", FileSize = 20566711  },
        new() { Platform = "win32", Channel = "release", Type = "game", Version = "2012.09.19.0001", BuildTime = new(2012, 09, 19), RepositoryHash = "48eca647", FileSize = 20874726  }
    ];
}
