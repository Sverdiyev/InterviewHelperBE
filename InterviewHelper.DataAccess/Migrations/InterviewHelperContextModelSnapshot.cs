﻿// <auto-generated />
using System;
using InterviewHelper.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InterviewHelper.DataAccess.Migrations
{
    [DbContext(typeof(InterviewHelperContext))]
    partial class InterviewHelperContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.4");

            modelBuilder.Entity("InterviewHelper.Core.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Complexity")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("EasyToGoogle")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<string>("QuestionContent")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Vote")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("InterviewHelper.Core.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("QuestionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TagName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("InterviewHelper.Core.Models.Tag", b =>
                {
                    b.HasOne("InterviewHelper.Core.Models.Question", null)
                        .WithMany("Tags")
                        .HasForeignKey("QuestionId");
                });

            modelBuilder.Entity("InterviewHelper.Core.Models.Question", b =>
                {
                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
