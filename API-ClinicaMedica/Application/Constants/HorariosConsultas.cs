namespace API_ClinicaMedica.Application.Constants;

public static class HorariosConsultas
{
    public static TimeSpan[] Horarios()
    {
        var inicio = new TimeSpan(8, 0, 0);
        var fim = new TimeSpan(18, 0, 0);

        var total = (int)(fim - inicio).TotalHours;

        var intervaloConsulta = TimeSpan.FromMinutes(30);

        var horarios = new List<TimeSpan>();

        var target = inicio;

        for (var i = 0; i < total * 2; i++)
        {
            horarios.Add(target + TimeSpan.FromHours(intervaloConsulta.TotalHours));
            target = target + TimeSpan.FromHours(intervaloConsulta.TotalHours);

        }

        return horarios.ToArray();

    }


}