using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Senluo.Spellet.Models;

namespace Senluo.Spellet.EF
{
    public class EFDbContext : DbContext
    {
        public EFDbContext() : base() { }

        public EFDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Answer>().ToTable("Answer");
            modelBuilder.Entity<AnswerSheet>().ToTable("AnswerSheet");
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<CourseContent>().ToTable("CourseContent");
            modelBuilder.Entity<CourseContent>().Ignore(o => o.Entry);
            modelBuilder.Entity<Entry>().ToTable("Entry");
            modelBuilder.Entity<Exam>().ToTable("Exam");
            modelBuilder.Entity<Example>().ToTable("Example");
            modelBuilder.Entity<Question>().ToTable("Question");
            modelBuilder.Entity<Question>().Ignore(o => o.Example);
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Teacher>().ToTable("Teacher");
            modelBuilder.Entity<Translation>().ToTable("Translation");

            
        }
    }
}